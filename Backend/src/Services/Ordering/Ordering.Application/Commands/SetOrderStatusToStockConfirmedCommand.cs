using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands
{
    [DataContract]
    public class SetOrderStatusToStockConfirmedCommand : IRequest
    {
        [DataMember]
        public int orderId { get; private set; }
        public SetOrderStatusToStockConfirmedCommand(int orderId)
        {
            this.orderId = orderId;
        }
    }
}
