using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
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
