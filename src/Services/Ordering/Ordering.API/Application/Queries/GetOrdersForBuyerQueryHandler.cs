using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetOrdersForBuyerQueryHandler: IRequestHandler<GetOrdersForBuyerQuery, List<OrderSummeryViewModel>>
    {
        private readonly IOrderQueries OrderQueries;

        public GetOrdersForBuyerQueryHandler(IOrderQueries orderQueries)
        {
            OrderQueries = orderQueries;
        }

        public async Task<List<OrderSummeryViewModel>> Handle(GetOrdersForBuyerQuery request, CancellationToken cancellationToken)
        {
            List<OrderSummeryViewModel> List = await OrderQueries.GetOrdersForBuyer(request.BuyerId);
            return List;
        }
    
    }
}
