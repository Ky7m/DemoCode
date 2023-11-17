using System.Collections.Generic;
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
    
    public void Increment(string productType)
    {
        _orderPlacedCounter.Add(1, new KeyValuePair<string, object>("product.type", productType));
    }
}