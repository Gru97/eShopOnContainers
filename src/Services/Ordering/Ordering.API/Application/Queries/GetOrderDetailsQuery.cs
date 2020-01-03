using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.API.Application.Queries
{
    public  class GetOrderDetailsQuery : IRequest<OrderViewModel>
    {

        public GetOrderDetailsQuery(int orderId)
        {
            this.OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}