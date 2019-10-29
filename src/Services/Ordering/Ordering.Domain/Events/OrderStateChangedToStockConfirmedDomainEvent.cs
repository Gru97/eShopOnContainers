using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Events
{
    public class OrderStateChangedToStockConfirmedDomainEvent:INotification
    {
        public int OrderId { get; }

        public OrderStateChangedToStockConfirmedDomainEvent(int orderId)
        {
            OrderId = orderId;
        }
        
    }
}
