using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Events
{
    public class OrderStartedDomainEvent
    {
        public string UserId { get;  }
        public string UserName { get;  }
        public Order Order { get; }

        public OrderStartedDomainEvent( Order order,string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            Order = order;
        }
    }
}
