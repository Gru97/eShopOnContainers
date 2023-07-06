using MediatR;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
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