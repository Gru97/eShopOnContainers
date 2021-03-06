﻿using BuildingBlocks.EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events
{
    public class OrderStockRejectedIntegrationEvent:IntegrationEvent
    {
        public int orderId { get; private set; }

        public OrderStockRejectedIntegrationEvent(int orderId)
        {
            this.orderId = orderId;
        }
    }
}