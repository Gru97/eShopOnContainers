using MediatR;
using Ordering.Domain;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
{
    public class GetAllOrdersQuery : IRequest<PagedResult<OrderSummeryViewModel>>
    {
        public int pageSize { get; set; }
        public int pageIndex { get; set; }

        public GetAllOrdersQuery(int pageSize, int pageIndex)
        {
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
        }
    }
}
