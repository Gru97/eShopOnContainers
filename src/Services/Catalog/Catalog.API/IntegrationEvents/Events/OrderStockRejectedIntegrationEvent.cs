using BuildingBlocks.EventBus.Events;
using System;

namespace Catalog.API.IntegrationEvents.Events
{
    internal class OrderStockRejectedIntegrationEvent:IntegrationEvent
    {
        public int orderId { get; private set; }
        public OrderStockRejectedIntegrationEvent(int orderId) : base(Guid.NewGuid(), DateTime.UtcNow)
        {
            this.orderId = orderId;
        }
    }
}