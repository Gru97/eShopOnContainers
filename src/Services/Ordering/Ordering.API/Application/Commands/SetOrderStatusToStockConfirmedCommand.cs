using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
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
