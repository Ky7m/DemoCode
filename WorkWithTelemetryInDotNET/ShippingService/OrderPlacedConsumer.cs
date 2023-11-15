using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;

namespace ShippingService;

[UsedImplicitly]
public class OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger) : IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        logger.LogInformation("ShippingService has received OrderPlaced, OrderId = {OrderId}", context.Message.OrderId);
        return Task.CompletedTask;
    }
}