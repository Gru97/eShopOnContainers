using BuildingBlocks.EventBus.Events;
using Catalog.API.IntegrationEvents.Events;
using System;

namespace Catalog.API.IntegrationEvents.Events
{
    public class OrderStockConfirmedIntegrationEvent:IntegrationEvent
    {
        public int orderId { get; private set; }
        public OrderStockConfirmedIntegrationEvent(int orderId) : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            this.orderId = orderId;
        }
    }
}