using EventBus.Abstractions;
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
    public class UserCheckoutIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutIntegrationEvent>
    {
        private readonly IMediator mediator;
        private ILogger<UserCheckoutIntegrationEventHandler> logger;

        public UserCheckoutIntegrationEventHandler(IMediator mediator,
            ILogger<UserCheckoutIntegrationEventHandler> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger;
        }

        public async Task Handle(UserCheckoutIntegrationEvent @event)
        {
            //Create an OrderCreatedCommand from @event and send through mediatR to it's Command handler

            logger.LogInformation("Inside UserCheckoutIntegrationEvent handler");
            var command = new CreateOrderCommand(@event.Basket.Items,
                @event.UserId, @event.UserName, @event.City,
                @event.State, @event.Country,
                @event.Street, @event.ZipCode);

            logger.LogInformation("CreateOrderCommand created {@command}", command);

            //dispatch it
            try
            {
                bool result= await mediator.Send(command);
                logger.LogInformation("CreateOrderCommand sent");


            }
            catch (Exception ex)
            {
                logger.LogError("CreateOrderCommand couldn't be sent"+ ex.Message);
                //throw new Exception(ex.Message);
            }
        }
    }
}
