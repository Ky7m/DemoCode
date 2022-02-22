using JetBrains.Annotations;
using NServiceBus;
using NServiceBus.Extensions.Diagnostics;
using Shared;

namespace BillingService;

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
        _logger.LogInformation("BillingService has received OrderPlaced, OrderId = {OrderId}", message.OrderId);
        var currentActivity = context.Extensions.Get<ICurrentActivity>();
        currentActivity.Current?.AddTag("payment.transaction.id", Guid.NewGuid().ToString());
        return Task.CompletedTask;
    }
}