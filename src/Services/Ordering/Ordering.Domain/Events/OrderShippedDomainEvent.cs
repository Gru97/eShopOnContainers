using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Events
{
    public class OrderShippedDomainEvent:INotification
    {
        public Order Order { get; }


        public OrderShippedDomainEvent(Order order)
        {
            Order = order;
        }
    }
}
