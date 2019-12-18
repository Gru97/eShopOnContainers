using EventBus.Abstractions;
using MediatR;
using Ordering.API.Application.Commands;
using Ordering.API.Application.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.IntegrationEvents.EventHandling
{
    public class OrderStockRejectedIntegrationEventHandler : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
    {
        private readonly IMediator mediator;

        public OrderStockRejectedIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(OrderStockRejectedIntegrationEvent @event)
        {
            var cmd = new SetOrderStatusToStockRejectedCommand(@event.orderId);
           await  mediator.Send(cmd);
        }
    }
}
