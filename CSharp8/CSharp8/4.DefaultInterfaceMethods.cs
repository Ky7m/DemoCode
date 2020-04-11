using System;

namespace CSharp8
{
    public interface IOrder
    {
        DateTime Purchased { get; }
        decimal Cost { get; }

        private static decimal discountPercent = 0.10m;
        public decimal ComputeLoyaltyDiscount() => DefaultLoyaltyDiscount(this);

        protected static decimal DefaultLoyaltyDiscount(IOrder order)
        {
            if (order.Purchased <= DateTime.Now.AddDays(-7.0))
            {
                return discountPercent;
            }

            return 0;
        }
    }

    public interface IOrderV2 : IOrder
    {
        public bool IsV2() => true;
    }

    public class SampleOrder : IOrderV2
    {
        public SampleOrder(DateTime purchase, decimal cost) =>
            (Purchased, Cost) = (purchase, cost);

        public DateTime Purchased { get; }

        public decimal Cost { get; }
    }
}