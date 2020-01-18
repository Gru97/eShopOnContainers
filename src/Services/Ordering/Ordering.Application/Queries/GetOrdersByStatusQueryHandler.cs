using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ordering.QueryModel;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
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
