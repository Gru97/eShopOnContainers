﻿using BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Catalog.API.IntegrationEvents.Events
{
    
    public class ProductPriceChangedIntegrationEvent:IntegrationEvent
    {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public ProductPriceChangedIntegrationEvent(int productId, decimal newPrice,decimal oldPrice)
            :base(Guid.NewGuid(),DateTime.UtcNow)
        {
            ProductId = productId;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }
}
