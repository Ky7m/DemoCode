using NServiceBus;

namespace Shared
{
    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
    }
}