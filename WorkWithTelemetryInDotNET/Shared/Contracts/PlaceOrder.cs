using System;

namespace Shared.Contracts
{
    public record PlaceOrder
    {
        public Guid OrderId { get; init; } 
    }
}