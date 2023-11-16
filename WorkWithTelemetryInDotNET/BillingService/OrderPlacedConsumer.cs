using System.Diagnostics;
using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;

namespace BillingService;

[UsedImplicitly]
public class OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger) : IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var paymentMethod = Random.Shared.GetItems(["CreditCard", "PayPal", "Invoice"],1)[0];
        var paymentTransactionInfo = new PaymentTransactionInfo(Guid.NewGuid(), paymentMethod);
        
        logger.LogOrderPaymentInfo(context.Message.OrderId, paymentTransactionInfo);
        
        Activity.Current?.AddTag("payment.transaction.id", paymentTransactionInfo.TransactionId);
        return Task.CompletedTask;
    }
}

public record PaymentTransactionInfo(Guid TransactionId, string PaymentMethod);

public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "BillingService has received OrderPlaced, OrderId = {OrderId}")]
    public static partial void LogOrderPaymentInfo(this ILogger logger, Guid orderId, [LogProperties] PaymentTransactionInfo paymentTransactionInfo);
}