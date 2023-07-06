using System.Threading.Tasks;
using BuildingBlocks.EventBus.Events;

namespace Ordering.Application.IntegrationEvents
{
    public interface IOrderingIntegrationEventService

    {
        Task AddAndSaveEventAsync(IntegrationEvent evt);
        void PublishEvent(IntegrationEvent evt);
    }
}
