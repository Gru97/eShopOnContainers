using Basket.API.IntegrationEvents.Events;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.IntegrationEvents.EventHandling
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<Events.ProductPriceChangedIntegrationEvent>
    {
        public Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
