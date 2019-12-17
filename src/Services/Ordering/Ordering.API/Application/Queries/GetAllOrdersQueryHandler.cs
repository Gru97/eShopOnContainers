using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderViewModel>>
    {
        private readonly IOrderQueries OrderQueries;
        public async Task<List<OrderViewModel>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var List= await OrderQueries.GetOrders();
            return List;
        }
    }
}
