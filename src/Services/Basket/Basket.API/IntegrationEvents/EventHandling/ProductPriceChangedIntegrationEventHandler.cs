using Basket.API.IntegrationEvents.Events;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Model;
using Newtonsoft.Json;

namespace Basket.API.IntegrationEvents.EventHandling
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<Events.ProductPriceChangedIntegrationEvent>
    {
        private readonly IBasketRepository repository;

        public ProductPriceChangedIntegrationEventHandler(IBasketRepository repository)
        {
            this.repository = repository;
        }
        public async Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            var users = await repository.GetAllUsersAsync();
            foreach (var Id in users)
            {
                var basket=await repository.GetBasketAsync(Id);
                //TODO SingleOrDefault or Where?
                var item = basket.Items.SingleOrDefault(x => x.Id == @event.ProductId);
                if (item != null)
                {
                    item.UnitPrice = @event.NewPrice;
                    item.OldPrice = @event.OldPrice;

                }


            }


        }
    }
}
