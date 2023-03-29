using System;

namespace Shared.Contracts
{
    public record OrderPlaced
    {
        public Guid OrderId { get; init; }
    }
}