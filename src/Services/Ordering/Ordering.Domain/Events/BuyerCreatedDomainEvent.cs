using System;
using EventStore;
using MediatR;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class BuyerCreatedDomainEvent :DomainEvent
    {
        public int orderId { get; private set; }
        public int buyerId { get; private set; }
        public string buyerIdentityGuid { get; private set; }
        public string buyerName { get; private set; }

        public BuyerCreatedDomainEvent(int orderId, int buyerId, string buyerName, string buyerIdentityGuid) :base(Guid.NewGuid(), DateTime.UtcNow )
        {
            this.orderId = orderId;
            this.buyerId = buyerId;
            this.buyerName = buyerName;
            this.buyerIdentityGuid = buyerIdentityGuid;
        }

    

    }
}