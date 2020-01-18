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
        public string UserId { get;  }
        public string UserName { get;  }
        public Order Order { get; }

        public OrderStartedDomainEvent( Order order,string userId, string userName) : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            UserId = userId;
            UserName = userName;
            Order = order;
        }
    }
}
