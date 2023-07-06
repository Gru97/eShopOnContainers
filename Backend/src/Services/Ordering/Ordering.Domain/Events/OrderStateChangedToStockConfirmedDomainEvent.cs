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
        public string Description { get; }

        public OrderStateChangedToStockConfirmedDomainEvent(int orderId, string description) 
            : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            OrderId = orderId;
            Description=description;
        }
        
    }
}
