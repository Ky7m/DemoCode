using System.Diagnostics;
using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;

namespace BillingService;

[UsedImplicitly]
public partial class OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger) : IConsumer<OrderPlaced>
{
    [LoggerMessage(Level = LogLevel.Information, Message = "BillingService has received OrderPlaced, OrderId = {OrderId}")]
    public static partial void LogOrderReceivedEvent(ILogger logger, Guid orderId);

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            LogOrderReceivedEvent(logger, context.Message.OrderId);
        }
        Activity.Current?.AddTag("payment.transaction.id", Guid.NewGuid().ToString());
        return Task.CompletedTask;
    }
}