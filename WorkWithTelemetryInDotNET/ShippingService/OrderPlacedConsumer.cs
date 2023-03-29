using JetBrains.Annotations;
using MassTransit;
using Shared.Contracts;

namespace ShippingService;

[UsedImplicitly]
public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    private readonly ILogger<OrderPlacedConsumer> _logger;

    public OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        _logger.LogInformation("ShippingService has received OrderPlaced, OrderId = {OrderId}", context.Message.OrderId);
        return Task.CompletedTask;
    }
}