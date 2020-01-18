using System;
using EventStore;
using MediatR;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class BuyerCreatedDomainEvent :DomainEvent, INotification
    {
        public int orderId { get; private set; }
        public int buyerId { get; private set; }

        public BuyerCreatedDomainEvent(int orderId, int buyerId):base(Guid.NewGuid(), DateTime.UtcNow )
        {
            this.orderId = orderId;
            this.buyerId = buyerId;
        }
    }
}