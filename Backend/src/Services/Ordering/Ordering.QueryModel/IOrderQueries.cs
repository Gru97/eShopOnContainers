using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore;
using Ordering.QueryModel.ViewModels;

namespace Ordering.QueryModel

{
    public interface IOrderQueries
    {
        //These methods are "Query Repository methods" 
        //They take some params and return ViewModels
        //Implementation is in another layer which is using any kind of ORM or data access technology

        Task Upsert(OrderDocument order);
        Task<OrderSummeryViewModel> GetOrderAsync(int orderId);
        Task<PagedResult<OrderSummeryViewModel>> GetOrders(int pageSize, int pageIndex);

        Task<List<OrderSummeryViewModel>> GetOrdersForBuyer(string buyerId);
        Task<PagedResult<OrderSummeryViewModel>> GetOrdersByStatus(int pageSize, int pageIndex, int status);
        Task<OrderViewModel> GetOrderDetails(int orderId);
        Task<OrderDocument> GetOrderDocument(int orderId);
    }
}
