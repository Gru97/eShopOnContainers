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
    public class UserCheckoutIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutIntegrationEvent>
    {
        private readonly IMediator mediator;
        
        public UserCheckoutIntegrationEventHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(UserCheckoutIntegrationEvent @event)
        {
            //Create an OrderCreatedCommand from @event and send through mediatR to it's Command handler


            var command = new CreateOrderCommand(@event.Basket.Items,
                @event.UserId, @event.UserName, @event.City,
                @event.State, @event.Country,
                @event.Street, @event.ZipCode);
            //dispatch it
            try
            {
                bool result= await mediator.Send(command);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
