using System.Threading.Tasks;
using BuildingBlocks.EventBus.Events;
using EventBus.Abstractions;
using IntegrationEventLogEF.Services;

namespace Ordering.Application.IntegrationEvents
{
    public class OrderingIntegrationEventService : IOrderingIntegrationEventService
    {
        private readonly IIntegrationEventLogService eventLogService;
        private readonly IEventBus eventBus;

        public OrderingIntegrationEventService(IIntegrationEventLogService eventLogService, IEventBus eventBus)
        {
            this.eventLogService = eventLogService;
            this.eventBus = eventBus;
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            await eventLogService.SaveEventAsync(evt);
        }

        public  void PublishEvent(IntegrationEvent evt)
        {
            eventBus.Publish(evt);
        }
    }
}
