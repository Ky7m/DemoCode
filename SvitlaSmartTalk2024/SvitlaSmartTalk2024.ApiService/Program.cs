using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddHttpContextAccessor();

builder.AddAzureBlobClient("BlobConnection");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/acceptTerms", async (
        [FromBody] AcceptTermsRequest request,
        [FromServices] BlobServiceClient blobServiceClient,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromServices] ILogger<Program> logger
    ) =>
    {
        var acceptedAt = DateTimeOffset.UtcNow;
        var virtualFolder = Sha256Hash(request.Email.ToLowerInvariant());
        var fileName = $"{virtualFolder}/{acceptedAt:yyyy-MM-ddTHH-mm-ssZ}.json";

        var fileData = new
        {
            request.FullName,
            request.Email,
            IPAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            AcceptedAtUtc = acceptedAt
        };

        try
        {
            await WriteAsync("acceptance-logs", fileName, fileData);
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Failed to write acceptance log to blob storage: {Data}", fileData);
        }

        return Results.Accepted();


        static string Sha256Hash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }
        
        async Task WriteAsync<T>(string containerName, string blobName, T blobData)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            var client = containerClient.GetBlockBlobClient(blobName);
            await using var ms = await client.OpenWriteAsync(true);
            await JsonSerializer.SerializeAsync(ms, blobData);
        }
    })
    .WithName("AcceptTerms");

app.MapDefaultEndpoints();

app.Run();

internal record AcceptTermsRequest(string FullName, string Email);
