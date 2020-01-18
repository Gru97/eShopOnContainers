using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using EventStore;

namespace Ordering.Domain.Events
{
    public class OrderStateChangedToStockConfirmedDomainEvent: DomainEvent,INotification
    {
        public int OrderId { get; }

        public OrderStateChangedToStockConfirmedDomainEvent(int orderId) 
            : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            OrderId = orderId;
        }
        
    }
}
