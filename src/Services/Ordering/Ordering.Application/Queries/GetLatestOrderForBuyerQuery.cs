using System.Runtime.Serialization;
using MediatR;
using Ordering.Domain;
using Ordering.QueryModel.ViewModels;

namespace Ordering.Application.Queries
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
