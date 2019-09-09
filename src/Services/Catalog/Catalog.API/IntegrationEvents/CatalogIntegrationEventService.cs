using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.EventBus.Events;
using Catalog.API.Infrastructure;
using  IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.IntegrationEvents
{
    public class CatalogIntegrationEventService:ICatalogIntegrationEventService
    {
        private readonly CatalogContext _catalogContext;
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public CatalogIntegrationEventService(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
            _integrationEventLogService = new IntegrationEventLogService(catalogContext.Database.GetDbConnection().ConnectionString);
        }
        public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
           await _catalogContext.SaveChangesAsync();
           await  _integrationEventLogService.SaveEventAsync(evt);
        }

       
    }
}
