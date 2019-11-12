using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderQueries
    {
        //These methods are "Query Repository methods" 
        //They take some params and return ViewModels
        //Implementation is in another layer which is using any kind of ORM or data access technology

        IEnumerable<OrderSummeryViewModel> GetOrders(Guid buyerId);
        OrderViewModel GetOrder(int Id);


    }
}
