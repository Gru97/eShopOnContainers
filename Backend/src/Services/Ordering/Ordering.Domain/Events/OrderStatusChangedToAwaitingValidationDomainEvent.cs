using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using EventStore;

namespace Ordering.Domain.Events
{
    //Event: Something that's happend in the past. Once created, can not be changed, hence no set (not even private)
    public class OrderStatusChangedToAwaitingValidationDomainEvent: DomainEvent,INotification
    {
        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId, IEnumerable<OrderItem> orderItems)
            : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }
}
