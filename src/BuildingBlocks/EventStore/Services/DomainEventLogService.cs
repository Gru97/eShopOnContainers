using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EventStore
{
    public class DomainEventLogService:IDomainEventLogService
    {
        private readonly DomainEventLogContext _eventLogContext;
        private string _dbConnection;

        public DomainEventLogService(string dbConnection)
        {
            _dbConnection = dbConnection;
            _eventLogContext = new DomainEventLogContext(
                new DbContextOptionsBuilder<DomainEventLogContext>()
                    .UseSqlServer(_dbConnection).Options
            );
        }
        public Task SaveEventAsync(DomainEvent @event)
        {
            var eventLogEntry = new DomainEventLogEntry(@event);
            //something about transaction
            _eventLogContext.DomainEventLogs.Add(eventLogEntry);
            return _eventLogContext.SaveChangesAsync();
        }
        public Task MarkEventAsRead(Guid evtId)
        {
           var current= _eventLogContext.DomainEventLogs.Single(e => e.EventId == evtId);
           current.TimeSent++;
           current.State = EventStateEnum.Read;
           _eventLogContext.DomainEventLogs.Update(current);
           return _eventLogContext.SaveChangesAsync();

        }

        public async Task<List<DomainEventLogEntry>> GetEventsAsync(int pageIndex, int pageSize,Expression<Func<DomainEventLogEntry,bool>> predicate)
        {
            var q=_eventLogContext.DomainEventLogs.Where(predicate).AsQueryable();
            return await q.Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();
        }
    }
}
