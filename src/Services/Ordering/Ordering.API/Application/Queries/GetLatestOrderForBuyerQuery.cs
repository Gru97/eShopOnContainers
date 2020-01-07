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
    public class GetLatestOrderForBuyerQuery:IRequest<OrderSummeryViewModel>
    {
        [DataMember]
        public string BuyerId { get; set; }

        public GetLatestOrderForBuyerQuery(string buyerId)
        {
            BuyerId = buyerId;
        }
    }
}
