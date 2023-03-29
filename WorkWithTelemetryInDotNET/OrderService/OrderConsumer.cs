using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;

namespace OrderService;

[UsedImplicitly]
public class OrderConsumer : IConsumer<PlaceOrder>, IConsumer<CancelOrder>
{
    private readonly ILogger<OrderConsumer> _logger;

    public OrderConsumer(ILogger<OrderConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        _logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", context.Message.OrderId);

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

        _logger.LogInformation("Publishing OrderPlaced, OrderId = {OrderId}", context.Message.OrderId);

        await context.Publish(orderPlaced);
    }

    public Task Consume(ConsumeContext<CancelOrder> context)
    {
        _logger.LogInformation("Received CancelOrder,OrderId = {OrderId}", context.Message.OrderId);
        return Task.CompletedTask;
    }
}