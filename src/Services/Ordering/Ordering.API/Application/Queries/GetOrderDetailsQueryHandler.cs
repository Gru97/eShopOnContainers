using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetOrderDetailsQueryHandler: IRequestHandler<GetOrderDetailsQuery, OrderViewModel>
    {
        private readonly IOrderQueries OrderQueries;

        public GetOrderDetailsQueryHandler(IOrderQueries orderQueries)
        {
            OrderQueries = orderQueries;
        }
    
        public async Task<OrderViewModel> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = await OrderQueries.GetOrderDetails(request.OrderId);
            return order;
        }
    }
}
