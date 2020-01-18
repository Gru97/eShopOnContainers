using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ordering.QueryModel;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
{
    public class GetLatestOrderForBuyerQueryHandler : IRequestHandler<GetLatestOrderForBuyerQuery, OrderSummeryViewModel>
    {
        private readonly IOrderQueries OrderQueries;

        public GetLatestOrderForBuyerQueryHandler(IOrderQueries orderQueries)
        {
            OrderQueries = orderQueries;
        }

        public async Task<OrderSummeryViewModel> Handle(GetLatestOrderForBuyerQuery request, CancellationToken cancellationToken)
        {
            var orders = await OrderQueries.GetOrdersForBuyer(request.BuyerId);
            if (orders != null && orders.Count > 0)
            {
                var latest = orders.First();
                var diff = DateTime.Now.Subtract(latest.date).Seconds;
                //if (diff < 50)
                    return latest;

            }
            return null;

        }
    }
}
