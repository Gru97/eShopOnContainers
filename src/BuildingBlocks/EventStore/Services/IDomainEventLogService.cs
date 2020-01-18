using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventStore
{
    public interface IDomainEventLogService
    {
        Task SaveEventAsync(DomainEvent @event);
        Task MarkEventAsRead(Guid evtId);

    }
}
