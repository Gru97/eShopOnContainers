using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents;
using Catalog.API.IntegrationEvents.Events;
using Catalog.API.Models;
using EventBus.Abstractions;
using IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly Infrastructure.CatalogContext _catalogContext;
        private IEventBus _eventBus;
        private ICatalogIntegrationEventService _catalogIntegrationEventService;
        private IIntegrationEventLogService _integrationEventLogService;

        public CatalogController(CatalogContext catalogContext, IEventBus eventBus, ICatalogIntegrationEventService catalogIntegrationEventService, IIntegrationEventLogService integrationEventLogService)
        {
            this._catalogContext = catalogContext;
            this._eventBus = eventBus;
            _catalogIntegrationEventService = catalogIntegrationEventService;
            _integrationEventLogService = integrationEventLogService;
        }

        
        
        [Route("items")]
        [HttpGet]
        public ActionResult<IEnumerable<CatalogItem>> GetAll()
        {
            //To test rabbitMQ
            //eventBus.Publish(new ProductPriceChangedIntegrationEvent(1, 10, 15) { });
            //var items = new List<CatalogItem> { }
            var items=_catalogContext.CatalogItems.ToList();
            ;
            return items;
        }

        [HttpPut]
        [Route("items")]
        public async Task UpdateProductAsync(CatalogItem productToUpdate)
        {
            //TODO: checking for necessity of publishing event PriceChanged
            //TODO: Other possibilities for state of event is not considered
            //To update the price of a product and guarantee consistency,
            //(that the publishing of it's related event has definitely happened after persisting in current service,
            //first we need to update CatalogContext(and product)
            //and we need to store details of the related event in database (IntegrationEventLogContext) 
            //IN THE SAME TRANSACTION
            //then we will publish the event
            //and if everything goes as expected, we change the state of event to published in db
            var oldPrice = _catalogContext.CatalogItems.Single(e => e.Id == productToUpdate.Id).Price;
            ProductPriceChangedIntegrationEvent evt=new ProductPriceChangedIntegrationEvent(productToUpdate.Id, productToUpdate.Price, oldPrice);
            using (var transaction = _catalogContext.Database.BeginTransaction())
            {
                _catalogContext.CatalogItems.Update(productToUpdate);
                await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(evt);
                transaction.Commit();
            }
            _eventBus.Publish(evt);
            await _integrationEventLogService.MarkEventAsPublished(evt.Id);

        }
    }
}