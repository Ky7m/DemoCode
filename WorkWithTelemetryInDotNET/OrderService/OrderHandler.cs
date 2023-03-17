using JetBrains.Annotations;
using Shared;

namespace OrderService;

[UsedImplicitly]
public class OrderHandler : IHandleMessages<PlaceOrder>, IHandleMessages<CancelOrder>
{
    private readonly ILogger<OrderHandler> _logger;

    public OrderHandler(ILogger<OrderHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
       _logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        // This is normally where some business logic would occur
        await Task.Delay(TimeSpan.FromSeconds(5), context.CancellationToken);

        // test throwing transient exceptions
        if (Random.Shared.Next(0, 5) == 0)
        {
            throw new Exception("Oops:" + message.OrderId);
        }

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };

        _logger.LogInformation("Publishing OrderPlaced, OrderId = {OrderId}", message.OrderId);

        await context.Publish(orderPlaced);
    }
        
    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Received CancelOrder,OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}