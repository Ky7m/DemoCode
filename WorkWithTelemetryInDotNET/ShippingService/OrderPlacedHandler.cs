using JetBrains.Annotations;
using NServiceBus;
using Shared;

namespace ShippingService;

[UsedImplicitly]
public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    private readonly ILogger<OrderPlacedHandler> _logger;

    public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        _logger.LogInformation("ShippingService has received OrderPlaced, OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}