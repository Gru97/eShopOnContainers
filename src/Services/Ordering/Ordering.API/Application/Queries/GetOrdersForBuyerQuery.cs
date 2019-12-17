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
    public class GetOrdersForBuyerQuery:IRequest<List<OrderSummeryViewModel>>
    {
        [DataMember]
        public string BuyerId { get; private set; }

        public GetOrdersForBuyerQuery(string buyerId)
        {
            BuyerId = buyerId;
        }
    }
}
