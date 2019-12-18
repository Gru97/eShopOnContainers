using BuildingBlocks.EventBus.Events;
using Catalog.API.IntegrationEvents.Events;

namespace Catalog.API.IntegrationEvents.Events
{
    internal class OrderStockConfirmedIntegrationEvent:IntegrationEvent
    {
        private int orderId;

        public OrderStockConfirmedIntegrationEvent(int orderId)
        {
            this.orderId = orderId;
        }
    }
}