using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure;

namespace Ordering.API.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly OrderingContext context;
        public OrderViewModel GetOrder(int Id)
        {
            var order = context.Orders.Where(e => e.Id == Id)
                .Include(e=>e.Address)
                .Include(e=>e.OrderItems).First();
            return new OrderViewModel()
            {
                city = order.Address.City,
                country = order.Address.Country,
                street = order.Address.Street,
                orderitems = order.OrderItems.Select(e=>ToOrderItemViewModel(e)).ToList(),
                zipcode = order.Address.ZipCode,
                ordernumber = order.Id,
                //TODO: problem with returning order status, I must store them
                //status = order.OrderState,
                total = order.OrderItems.Sum(e => e.Quantity * e.UnitPrice)
                //TODO: we have problem filling date and description becasue they are private. I need to create them as backing fields later
            };
        }

        private OrderItemViewModel ToOrderItemViewModel(OrderItem e)
        {
            return new OrderItemViewModel() {productname=e.ProductName,
            unitprice=e.UnitPrice,
            units=e.Quantity};
        }

        public IEnumerable<OrderSummeryViewModel> GetOrders(Guid buyerId)
        {
            var q=from o in context.Orders join b in context.Buyers
                  on o.BuyerId equals b.Id
                  where b.IdentityGuid==buyerId.ToString()
                  select new OrderSummeryViewModel
                  {
                      ordernumber=o.Id,
                      total=o.OrderItems.Sum(e=>e.UnitPrice*e.Quantity)
                  };

            return q;
        }
    }
}
