using NServiceBus;

namespace Shared
{
    public class CancelOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}