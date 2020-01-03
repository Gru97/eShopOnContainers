using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetOrdersByStatusQueryHandler : IRequestHandler<GetOrdersByStatusQuery, PagedResult<OrderSummeryViewModel>>
    {
        IOrderQueries orderQueries;

        public GetOrdersByStatusQueryHandler(IOrderQueries orderQueries)
        {
            this.orderQueries = orderQueries;
        }

        public Task<PagedResult<OrderSummeryViewModel>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
        {
            return orderQueries.GetOrdersByStatus(request.PageSize, request.PageIndex, request.Status);
        }
    }
}
