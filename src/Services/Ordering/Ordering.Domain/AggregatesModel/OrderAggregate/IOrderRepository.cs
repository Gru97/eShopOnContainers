using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    // Only the repository contract is defined in Domain layer
    // The implementaion is the Infrastructure layer consern
    public interface IOrderRepository:IRepository<Order>
    {
    }
}
