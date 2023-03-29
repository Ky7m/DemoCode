using MassTransit;
using Shared.Contracts;

namespace OrderGenerator;

public class OrderGeneratorWorker : BackgroundService
{
    private readonly ILogger<OrderGeneratorWorker> _logger;
    private readonly IBus _bus;

    public OrderGeneratorWorker(ILogger<OrderGeneratorWorker> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = Random.Shared;
        while (!stoppingToken.IsCancellationRequested)
        {
            var orderId = Guid.NewGuid();

            await PlaceOrder(orderId);

            if (random.Next(0, 5) == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                await CancelOrder(orderId);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private Task PlaceOrder(Guid orderId)
    {
        var message = new PlaceOrder
        {
            OrderId = orderId
        };
        _logger.LogInformation("Sending PlaceOrder, OrderId = {OrderId}", orderId);
        return _bus.Publish(message);
    }

    private Task CancelOrder(Guid orderId)
    {
        var message = new CancelOrder
        {
            OrderId = orderId
        };
        _logger.LogInformation("Sending CancelOrder, OrderId = {OrderId}", orderId);
        return _bus.Publish(message);
    }
}