using MediatR;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class BuyerCreatedDomainEvent : INotification
    {
        public int orderId { get; private set; }
        public int buyerId { get; private set; }

        public BuyerCreatedDomainEvent(int orderId, int buyerId)
        {
            this.orderId = orderId;
            this.buyerId = buyerId;
        }
    }
}