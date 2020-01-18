using BuildingBlocks.EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events
{
    public class OrderStockConfirmedIntegrationEvent:IntegrationEvent
    {
        public int orderId { get; set; }

        public OrderStockConfirmedIntegrationEvent(int orderId)
        {
            this.orderId = orderId;
        }
    }
}