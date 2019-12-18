using BuildingBlocks.EventBus.Events;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents.Events;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.IntegrationEvents.EventHandling
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler
        : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly Infrastructure.CatalogContext _catalogContext;
        private readonly IEventBus _eventBus;

        public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(CatalogContext catalogContext, IEventBus eventBus)
        {
            _catalogContext = catalogContext;
            _eventBus = eventBus;
        }

        public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
        {
            //Check item stocks. 
            List<ConfirmedStockItem> confirmedItems = new List<ConfirmedStockItem>();
            foreach (var item in @event.StockItems)
            {
                
                var product=await _catalogContext.CatalogItems.FindAsync(item.ProductId);
                 confirmedItems.Add(
                     new ConfirmedStockItem
                     { ProductId=item.ProductId,HasStock= product.AvailableStock >= item.Units });

            }
            //If they are available publish an event accepting the order, if not reject the order 

            var resultEvent=confirmedItems.Any(c=>!c.HasStock) ?
              (IntegrationEvent)  new OrderStockRejectedIntegrationEvent(@event.OrderId) : new OrderStockConfirmedIntegrationEvent(@event.OrderId);

            //TODO: save event to eventLog
            _eventBus.Publish(resultEvent);



        }
        public class ConfirmedStockItem
        {
            public int ProductId { get; set; }
            public bool HasStock { get; set; }
        }
    }
}
