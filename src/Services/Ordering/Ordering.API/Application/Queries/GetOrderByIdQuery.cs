using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
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
