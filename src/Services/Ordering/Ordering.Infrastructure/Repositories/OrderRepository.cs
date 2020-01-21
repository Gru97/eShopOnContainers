using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext context;
        
        public IUnitOfWork UnitOfWork
        {
            get {return context;}
        }

        public OrderRepository(OrderingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order Add(Order order)
        {
            return context.Add(order).Entity;
        }

        public Order Get(int orderId)
        {
            //We could use Find and then explicitly load related data. That way we might hava saved one round trip to db
            //since SingleOrDefault forcably queries database but Find queries memory 
            var order = context.Orders
                .Include(e => e.Address)
                .Include(e => e.OrderItems)
                .SingleOrDefault(e => e.Id == orderId);

            return order;
            
        }

        public async Task<Order> FindAsycn(int orderId)
        {

            var order = await context.Orders.FindAsync(orderId);
            if (order != null)
            {
                await context.Entry(order)
                    .Collection(i => i.OrderItems).LoadAsync();
                
                await context.Entry(order)
                    .Reference(i => i.Address).LoadAsync();
            }

            return order;


        }


        public void Update(Order order)
        {
            context.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
