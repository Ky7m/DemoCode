using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;
using Shared.Metrics;

namespace OrderService;

[UsedImplicitly]
public class OrderConsumer(ILogger<OrderConsumer> logger, OrderMetrics orderMetrics) : IConsumer<PlaceOrder>, IConsumer<CancelOrder>
{
    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", context.Message.OrderId);
        orderMetrics.Increment();

        // This is normally where some business logic would occur
        await Task.Delay(TimeSpan.FromSeconds(5), context.CancellationToken);

        // test throwing transient exceptions
        if (Random.Shared.Next(0, 5) == 0)
        {
            throw new Exception("Oops:" + context.Message.OrderId);
        }

        var orderPlaced = new OrderPlaced
        {
            OrderId = context.Message.OrderId
        };

        logger.LogInformation("Publishing OrderPlaced, OrderId = {OrderId}", context.Message.OrderId);

        await context.Publish(orderPlaced);
    }

    public Task Consume(ConsumeContext<CancelOrder> context)
    {
        logger.LogInformation("Received CancelOrder,OrderId = {OrderId}", context.Message.OrderId);
        return Task.CompletedTask;
    }
}