using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ordering.QueryModel;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
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
            var orderSummeryViewModel = await OrderQueries.GetOrderAsync(request.OrderId);
            return orderSummeryViewModel;
        }
    
    }
}
