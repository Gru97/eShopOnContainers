using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventStore;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    // Only the repository contract is defined in Domain layer
    // The implementaion is the Infrastructure layer consern
    
    public interface IOrderRepository:IRepository<Order>
    {
        Order Add(Order order);
        void Update(Order order);

        //Simple queries only. Complex ones go to "query layer" of the cqrs pattern
        Order Get(int orderId);
        Task<Order> FindAsycn(int orderId);

    }
}
