using System;

namespace Shared.Contracts
{
    public record CancelOrder
    {
        public Guid OrderId { get; init; }
    }
}