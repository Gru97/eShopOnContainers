using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventStore
{
    public interface IDomainEventLogService
    {
        Task SaveEventAsync(DomainEvent @event);
        Task MarkEventAsRead(Guid evtId);

        Task<List<DomainEventLogEntry>> GetEventsAsync(int pageIndex, int pageSize,
            Expression<Func<DomainEventLogEntry, bool>> predicate);


    }
}
