using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.EventBus.Events;

namespace IntegrationEventLogEF.Services
{
    public interface IIntegrationEventLogService
    {
        Task SaveEventAsync(IntegrationEvent @event);
        Task MarkEventAsPublished(Guid evtId);

    }
}
