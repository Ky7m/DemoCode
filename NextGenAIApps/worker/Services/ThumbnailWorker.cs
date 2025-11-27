using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;
using Worker.Data;

namespace Worker.Services;

public class ThumbnailWorker(
    IServiceProvider serviceProvider,
    BlobContainerClient containerClient,
    QueueServiceClient queueService,
    IHostApplicationLifetime hostApplicationLifetime,
    IConfiguration configuration,
    ILogger<ThumbnailWorker> logger) : BackgroundService
{
    private const int ThumbnailWidth = 300;
    private const int ThumbnailHeight = 300;
    private const long MaxImageSizeBytes = 20 * 1024 * 1024; // 20 MB - slightly larger than upload limit
    private const int MaxRetryCount = 3;
    private const int MaxEmptyPolls = 2;        // Poll up to 2 times (event-triggered)
    private const int EmptyPollWaitSeconds = 5; // Wait 5 seconds between polls (total: ~5 seconds)

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (configuration.GetValue<bool>("WORKER_RUN_CONTINUOUSLY"))
        {
            await ExecuteContinuousAsync(stoppingToken);
        }
        else
        {
            try
            {
                await ExecuteScheduledAsync(stoppingToken);
            }
            finally
            {
                logger.LogInformation("Shutting down worker application");
                hostApplicationLifetime.StopApplication();
            }
        }
    }

    private async Task ExecuteContinuousAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Thumbnail worker started in CONTINUOUS mode (local dev)");

        var queueClient = queueService.GetQueueClient("thumbnails");
        await queueClient.CreateIfNotExistsAsync(cancellationToken: stoppingToken);

        var processedCount = 0;
        var startTime = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await queueClient.ReceiveMessagesAsync(
                maxMessages: 10,
                visibilityTimeout: TimeSpan.FromMinutes(5),
                cancellationToken: stoppingToken);

            var messages = response.Value;

            if (messages.Length == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }

            logger.LogInformation("Found {Count} messages to process", messages.Length);

            foreach (var message in messages)
            {
                await ProcessMessageWithRetryAsync(message, queueClient, stoppingToken);
                processedCount++;
            }
        }

        logger.LogInformation("Thumbnail worker stopped. Total processed: {Count}, Duration: {Elapsed}",
            processedCount, DateTime.UtcNow - startTime);
    }

    private async Task ExecuteScheduledAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Thumbnail worker started in EVENT-TRIGGERED mode");

        var queueClient = queueService.GetQueueClient("thumbnails");
        await queueClient.CreateIfNotExistsAsync(cancellationToken: stoppingToken);

        var processedCount = 0;
        var emptyPollCount = 0;
        var startTime = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await queueClient.ReceiveMessagesAsync(
                maxMessages: 10,
                visibilityTimeout: TimeSpan.FromMinutes(5),
                cancellationToken: stoppingToken);

            var messages = response.Value;

            if (messages.Length == 0)
            {
                emptyPollCount++;

                if (emptyPollCount >= MaxEmptyPolls)
                {
                    logger.LogInformation("Queue empty after {PollCount} attempts, exiting. Processed {Count} messages in {Elapsed}",
                        emptyPollCount, processedCount, DateTime.UtcNow - startTime);
                    break;
                }

                logger.LogInformation("Queue empty, waiting {WaitSeconds}s before retry ({PollCount}/{MaxPolls})",
                    EmptyPollWaitSeconds, emptyPollCount, MaxEmptyPolls);
                await Task.Delay(TimeSpan.FromSeconds(EmptyPollWaitSeconds), stoppingToken);
                continue;
            }

            emptyPollCount = 0;
            logger.LogInformation("Found {Count} messages to process", messages.Length);

            foreach (var message in messages)
            {
                await ProcessMessageWithRetryAsync(message, queueClient, stoppingToken);
                processedCount++;
            }
        }

        logger.LogInformation("Thumbnail worker stopped. Total processed: {Count}, Duration: {Elapsed}",
            processedCount, DateTime.UtcNow - startTime);
    }

    private async Task ProcessMessageWithRetryAsync(
        QueueMessage message,
        QueueClient queueClient,
        CancellationToken cancellationToken)
    {
        try
        {
            await ProcessMessageAsync(message, queueClient, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process message: {MessageId}", message.MessageId);

            if (message.DequeueCount >= MaxRetryCount)
            {
                logger.LogWarning("Message {MessageId} exceeded max retry count ({MaxRetryCount}), deleting",
                    message.MessageId, MaxRetryCount);
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
            }
        }
    }

    private async Task ProcessMessageAsync(
        QueueMessage message,
        QueueClient queueClient,
        CancellationToken cancellationToken)
    {
        var startTime = DateTime.UtcNow;

        // Parse message
        var data = JsonSerializer.Deserialize<JsonElement>(message.MessageText);
        var imageId = data.GetProperty("imageId").GetInt32();
        var blobName = data.GetProperty("blobName").GetString()!;

        logger.LogInformation("Processing thumbnail for image {ImageId}, blob: {BlobName}", imageId, blobName);

        var sourceBlobClient = containerClient.GetBlobClient(blobName);

        // Check blob size before downloading
        var properties = await sourceBlobClient.GetPropertiesAsync(cancellationToken: cancellationToken);
        if (properties.Value.ContentLength > MaxImageSizeBytes)
        {
            logger.LogWarning("Image {ImageId} exceeds max size ({Size} bytes), skipping thumbnail generation",
                imageId, properties.Value.ContentLength);
            throw new InvalidOperationException($"Image size {properties.Value.ContentLength} exceeds maximum allowed {MaxImageSizeBytes}");
        }

        // Download original image
        using var originalStream = new MemoryStream();
        await sourceBlobClient.DownloadToAsync(originalStream, cancellationToken);
        originalStream.Position = 0;

        // Generate thumbnail
        using var image = await Image.LoadAsync(originalStream, cancellationToken);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(ThumbnailWidth, ThumbnailHeight),
            Mode = ResizeMode.Max
        }));

        // Upload thumbnail
        var thumbnailName = $"thumb-{blobName}";
        var thumbnailBlobClient = containerClient.GetBlobClient(thumbnailName);

        using var thumbnailStream = new MemoryStream();
        await image.SaveAsJpegAsync(thumbnailStream, cancellationToken);
        thumbnailStream.Position = 0;
        await thumbnailBlobClient.UploadAsync(thumbnailStream, overwrite: true, cancellationToken: cancellationToken);

        // Update database
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ImageDbContext>();
        var imageRecord = await db.Images.FindAsync([imageId], cancellationToken: cancellationToken);

        if (imageRecord != null)
        {
            imageRecord.ThumbnailUrl = thumbnailBlobClient.Uri.ToString();
            imageRecord.ThumbnailProcessed = true;
            await db.SaveChangesAsync(cancellationToken);
        }

        // Delete message from queue
        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);

        var processingTime = DateTime.UtcNow - startTime;
        logger.LogInformation(
            "Thumbnail generated for image {ImageId} in {ProcessingTime}ms",
            imageId,
            processingTime.TotalMilliseconds);
    }
}
