using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using EventStore;

namespace Ordering.Domain.Events
{
    public class OrderStateChangedToPaidDomainEvent: DomainEvent,INotification
    {
        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStateChangedToPaidDomainEvent(int orderId, IEnumerable<OrderItem> orderItems) 
            : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }
}
