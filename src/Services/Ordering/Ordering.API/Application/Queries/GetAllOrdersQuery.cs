using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
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
