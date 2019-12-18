using BuildingBlocks.EventBus.Events;

namespace Catalog.API.IntegrationEvents.Events
{
    internal class OrderStockRejectedIntegrationEvent:IntegrationEvent
    {
        private int orderId;

        public OrderStockRejectedIntegrationEvent(int orderId)
        {
            this.orderId = orderId;
        }
    }
}