using System.Runtime.Serialization;
using MediatR;
using Ordering.Domain;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
{
    [DataContract]
    public class GetOrderByIdQuery : IRequest<OrderSummeryViewModel>
    {
        [DataMember]
        public int OrderId { get; private set; }

        public GetOrderByIdQuery(int orderId)
        {
            OrderId = orderId;
        }
    }
}
