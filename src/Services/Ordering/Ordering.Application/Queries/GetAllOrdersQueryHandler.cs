using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ordering.QueryModel;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResult<OrderSummeryViewModel>>
    {
        private readonly IOrderQueries OrderQueries;

        public GetAllOrdersQueryHandler(IOrderQueries orderQueries)
        {
            OrderQueries = orderQueries;
        }

        public async Task<PagedResult<OrderSummeryViewModel>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {

            PagedResult <OrderSummeryViewModel> List = await OrderQueries.GetOrders(request.pageSize,request.pageIndex);
            return List;
        }
    }
}
