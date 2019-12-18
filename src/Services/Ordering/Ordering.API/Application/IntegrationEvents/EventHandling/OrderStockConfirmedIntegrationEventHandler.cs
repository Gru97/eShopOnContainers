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
    public class OrderStockConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
    {
        private readonly IMediator mediator;

        public OrderStockConfirmedIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
        {
            var cmd = new SetOrderStatusToStockConfirmedCommand(@event.orderId);
            await mediator.Send(cmd);
        }
    }
}
