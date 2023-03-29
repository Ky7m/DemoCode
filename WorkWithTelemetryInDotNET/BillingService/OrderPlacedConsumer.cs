using System.Diagnostics;
using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;

namespace BillingService;

[UsedImplicitly]
public partial class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    private readonly ILogger<OrderPlacedConsumer> _logger;

    public OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger)
    {
        _logger = logger;
    }
    
    [LoggerMessage(Level = LogLevel.Information, Message = "BillingService has received OrderPlaced, OrderId = {OrderId}")]
    public static partial void LogOrderReceivedEvent(ILogger logger, Guid orderId);

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            LogOrderReceivedEvent(_logger, context.Message.OrderId);
        }
        Activity.Current?.AddTag("payment.transaction.id", Guid.NewGuid().ToString());
        return Task.CompletedTask;
    }
}