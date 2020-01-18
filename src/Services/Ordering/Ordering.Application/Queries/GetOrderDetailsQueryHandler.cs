using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ordering.QueryModel;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
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
