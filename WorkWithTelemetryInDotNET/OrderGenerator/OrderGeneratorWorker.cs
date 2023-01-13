using Shared;

namespace OrderGenerator;

public class OrderGeneratorWorker : BackgroundService
{
    private readonly ILogger<OrderGeneratorWorker> _logger;
    private readonly IMessageSession _messageSession;

    public OrderGeneratorWorker(ILogger<OrderGeneratorWorker> logger, IMessageSession messageSession)
    {
        _logger = logger;
        _messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = Random.Shared;
        while (!stoppingToken.IsCancellationRequested)
        {
            var orderId = Guid.NewGuid().ToString();

            await PlaceOrder(orderId);

            if (random.Next(0, 5) == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                await CancelOrder(orderId);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private Task PlaceOrder(string orderId)
    {
        var command = new PlaceOrder
        {
            OrderId = orderId
        };
        _logger.LogInformation("Sending PlaceOrder command, OrderId = {OrderId}", orderId);
        return _messageSession.Send(command);
    }

    private Task CancelOrder(string orderId)
    {
        var command = new CancelOrder
        {
            OrderId = orderId
        };
        _logger.LogInformation("Sending CancelOrder command,OrderId = {OrderId}", orderId);
        return _messageSession.Send(command);
    }
}