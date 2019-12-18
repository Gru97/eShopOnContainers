using BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.IntegrationEvents.Events
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEvent:IntegrationEvent
    {

        public List<StockItem> StockItems { get;private set; }
        public int OrderId { get; private set; }

        public OrderStatusChangedToAwaitingValidationIntegrationEvent(List<StockItem> stockItems,
            int orderId)
        {
            StockItems = stockItems;
            OrderId = orderId;
        }
    }
    public class StockItem
    {
        public int ProductId { get; set; }
        public int Units { get; set; }
    }
}
