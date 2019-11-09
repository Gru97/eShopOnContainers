using Basket.API.IntegrationEvents.Events;
using Basket.API.Model;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.IntegrationEvents.EventHandling
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IBasketRepository basketRepository;

        public OrderStartedIntegrationEventHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        }

        public async Task Handle(OrderStartedIntegrationEvent @event)
        {
            await basketRepository.DeleteBasketAsync(@event.UserId);
        }
    }
}
