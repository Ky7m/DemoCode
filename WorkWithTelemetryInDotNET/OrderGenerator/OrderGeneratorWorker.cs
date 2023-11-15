using MassTransit;
using Shared.Contracts;

namespace OrderGenerator;

public class OrderGeneratorWorker(ILogger<OrderGeneratorWorker> logger, IBus bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = Random.Shared;
        while (!stoppingToken.IsCancellationRequested)
        {
            var orderId = Guid.NewGuid();

            await PlaceOrder(orderId);

            if (random.Next(0, 5) == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                await CancelOrder(orderId);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(500), stoppingToken);
        }
    }

    private Task PlaceOrder(Guid orderId)
    {
        var message = new PlaceOrder
        {
            OrderId = orderId
        };
        logger.LogInformation("Sending PlaceOrder, OrderId = {OrderId}", orderId);
        return bus.Publish(message);
    }

    private Task CancelOrder(Guid orderId)
    {
        var message = new CancelOrder
        {
            OrderId = orderId
        };
        logger.LogInformation("Sending CancelOrder, OrderId = {OrderId}", orderId);
        return bus.Publish(message);
    }
}