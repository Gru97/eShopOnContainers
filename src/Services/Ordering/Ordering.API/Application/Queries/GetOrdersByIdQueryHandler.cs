using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderSummeryViewModel>
    {
        private readonly IOrderQueries OrderQueries;

        public GetOrderByIdQueryHandler(IOrderQueries orderQueries)
        {
            OrderQueries = orderQueries;
        }

        public async Task<OrderSummeryViewModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderSummeryViewModel = await OrderQueries.GetOrder(request.OrderId);
            return orderSummeryViewModel;
        }
    
    }
}
