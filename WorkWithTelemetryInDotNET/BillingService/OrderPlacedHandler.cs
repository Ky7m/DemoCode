using JetBrains.Annotations;
using NServiceBus;
using NServiceBus.Extensions.Diagnostics;
using Shared;

namespace BillingService;

[UsedImplicitly]
public partial class OrderPlacedHandler : IHandleMessages<OrderPlaced>
{
    private readonly ILogger<OrderPlacedHandler> _logger;

    public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger)
    {
        _logger = logger;
    }
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "BillingService has received OrderPlaced, OrderId = {OrderId}")]
    public static partial void LogOrderReceivedEvent(ILogger logger, string orderId);

    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        LogOrderReceivedEvent(_logger, message.OrderId);
        var currentActivity = context.Extensions.Get<ICurrentActivity>();
        currentActivity.Current?.AddTag("payment.transaction.id", Guid.NewGuid().ToString());
        return Task.CompletedTask;
    }
}