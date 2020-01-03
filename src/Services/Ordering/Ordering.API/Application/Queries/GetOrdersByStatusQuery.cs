using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.API.Application.Queries
{
    public class GetOrdersByStatusQuery : IRequest<PagedResult<OrderSummeryViewModel>>
    {
        

        public GetOrdersByStatusQuery(int pageSize, int pageIndex, int status)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Status = status;
        }

        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        public int Status { get; private set; }
    }
}