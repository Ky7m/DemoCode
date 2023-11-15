using System.Diagnostics.Metrics;

namespace Shared.Metrics;

public sealed class OrderMetrics
{
    public const string MeterName = "OrderMetrics";
    
    private readonly Counter<int> _orderPlacedCounter;

    public OrderMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _orderPlacedCounter = meter.CreateCounter<int>("Placed Orders", "{orders}", "Number of orders placed");
    }
    
    public void Increment()
    {
        _orderPlacedCounter.Add(1);
    }
}