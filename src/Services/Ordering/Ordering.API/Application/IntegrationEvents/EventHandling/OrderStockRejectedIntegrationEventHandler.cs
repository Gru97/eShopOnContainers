﻿using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private ILogger<OrderStockRejectedIntegrationEventHandler> logger;


        public OrderStockRejectedIntegrationEventHandler(IMediator mediator,
            ILogger<OrderStockRejectedIntegrationEventHandler> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task Handle(OrderStockRejectedIntegrationEvent @event)
        {
            var cmd = new SetOrderStatusToStockRejectedCommand(@event.orderId);
            logger.LogInformation("SetOrderStatusToStockRejectedCommand created { @cmd}",cmd);

            await mediator.Send(cmd);
            logger.LogInformation("SetOrderStatusToStockRejectedCommand sent");
        }
    }
}
