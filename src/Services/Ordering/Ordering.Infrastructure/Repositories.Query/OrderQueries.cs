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

        public OrderQueries(OrderingContext context)
        {
            this.context = context;
        }

        public async Task<OrderSummeryViewModel> GetOrder(int Id)
        {
            var order = context.Orders
                .SingleOrDefault(e => e.Id == Id);
            if (order == null)
                return null;
            return ToOrderSummeryViewModel(order);
                
                
                
           
        }
       
        public async Task<PagedResult<OrderSummeryViewModel>> GetOrders(int pageSize,int pageIndex)
        {
            var order = context.Orders.AsQueryable();
            int recordCount = order.Count();

            order = order
                .Include(e => e.Address)
                .Include(e => e.OrderItems)
                .Skip(pageIndex * pageSize).Take(pageSize);
            var list =order.Select(ToOrderSummeryViewModel).ToList();
            return new PagedResult<OrderSummeryViewModel> { Count = recordCount, Items = list };
            
        }
     
        public async Task<List<OrderSummeryViewModel>> GetOrdersForBuyer(string buyerId)
        {
            var q=from o in context.Orders join b in context.Buyers
                  on o.BuyerId equals b.Id
                  where b.IdentityGuid==buyerId.ToString()
                  select new OrderSummeryViewModel
                  {
                      ordernumber=o.Id,
                      total=o.OrderItems.Sum(e=>e.UnitPrice*e.Quantity),
                      date=o.OrderDate,
                      status=o.OrderState.ToString()
                  };

            return q.ToList();
        }

        public async Task<PagedResult<OrderSummeryViewModel>> GetOrdersByStatus(int pageSize, int pageIndex, int status)
        {
            var q = context.Orders.AsQueryable();
            q = q.Where(e => e.OrderState == (OrderState) status)
                .Include(e=>e.OrderItems);
            
            int recordCount=q.Count();
            var list = q.Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .OrderBy(e => e.OrderDate)
                        .Select(ToOrderSummeryViewModel)
                        .ToList();
            return new PagedResult<OrderSummeryViewModel> {Count=recordCount,Items=list};

        }
        //private OrderViewModel ToOrderViewModel(Order order)
        //{
        //    return new OrderViewModel()
        //    {
        //        city = order.Address.City,
        //        country = order.Address.Country,
        //        street = order.Address.Street,
        //        orderitems = order.OrderItems.Select(e => ToOrderItemViewModel(e)).ToList(),
        //        zipcode = order.Address.ZipCode,
        //        ordernumber = order.Id,
        //        //TODO: problem with returning order status, I must store them
        //        //status = order.OrderState,
        //        total = order.OrderItems.Sum(e => e.Quantity * e.UnitPrice)
        //        //TODO: we have problem filling date and description becasue they are private. I need to create them as backing fields later
        //    };
        //}
        private OrderSummeryViewModel ToOrderSummeryViewModel(Order o)
        {
            return new OrderSummeryViewModel
            {
                ordernumber = o.Id,
                total = o.OrderItems.Sum(e => e.UnitPrice * e.Quantity),
                date=o.OrderDate,
                status=o.OrderState.ToString()
           };
        }
        private OrderItemViewModel ToOrderItemViewModel(OrderItem e)
        {
            return new OrderItemViewModel()
            {
                productname = e.ProductName,
                unitprice = e.UnitPrice,
                units = e.Quantity
            };
        }

        public async Task<OrderViewModel> GetOrderDetails(int Id)
        {
            string seperator = "-";
            var order = context.Orders.Where(e => e.Id == Id)
                .Include(e => e.Address)
                .Include(e => e.OrderItems).First();
            var buyer = context.Buyers.Where(e => e.Id == order.BuyerId).First();
            return new OrderViewModel()
            {
                address = order.Address.Country + seperator
                        + order.Address.City + seperator
                        + order.Address.Street + seperator
                        + order.Address.State + seperator
                        + order.Address.ZipCode,
                orderitems = order.OrderItems.Select(e => ToOrderItemViewModel(e)).ToList(),
                buyername=buyer.Name,
                date=order.OrderDate,
                ordernumber = order.Id,
                //TODO: problem with returning order status, I must store them
                //status = order.OrderState,
                total = order.OrderItems.Sum(e => e.Quantity * e.UnitPrice)
                //TODO: we have problem filling date and description becasue they are private. I need to create them as backing fields later
            };
        }
    }
}
