using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using EventStore;

namespace Ordering.Domain.Events
{
    public class OrderStartedDomainEvent: DomainEvent,INotification
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Order Order { get; set; }

        public OrderStartedDomainEvent( Order order,string userId, string userName) : base(new Guid(), DateTime.UtcNow)
        {
            UserId = userId;
            UserName = userName;
            Order = order;
        }
    }
}
