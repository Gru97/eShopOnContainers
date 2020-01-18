using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using EventStore;

namespace Ordering.Domain.Events
{
    public class OrderShippedDomainEvent: DomainEvent,INotification
    {
        public Order Order { get; }


        public OrderShippedDomainEvent(Order order) : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            Order = order;
        }
    }
}
