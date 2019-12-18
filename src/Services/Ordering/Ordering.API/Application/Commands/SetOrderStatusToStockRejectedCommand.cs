using MediatR;
using System;
using System.Runtime.Serialization;

namespace Ordering.API.Application.Commands
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