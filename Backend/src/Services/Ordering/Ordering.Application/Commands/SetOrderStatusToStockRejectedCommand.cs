using System.Runtime.Serialization;
using MediatR;

namespace Ordering.Application.Commands
{
    [DataContract]
    public class SetOrderStatusToStockRejectedCommand: IRequest
    {
        [DataMember]
        public int orderId { get; private set; }
        public SetOrderStatusToStockRejectedCommand(int orderId)
        {
            this.orderId = orderId;
        }
    }
}