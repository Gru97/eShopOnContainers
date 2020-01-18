using BuildingBlocks.EventBus.Events;

namespace Ordering.Application.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent:IntegrationEvent
    {
        public string UserId { get; set; }

        public OrderStartedIntegrationEvent(string userId)
        {
            UserId = userId;
        }
    }
}
