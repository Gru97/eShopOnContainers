using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF.Services
{
    public class IntegrationEventLogService:IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _eventLogContext;
        private string _dbConnection;

        public IntegrationEventLogService(string dbConnection)
        {
            _dbConnection = dbConnection;
            _eventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection).Options
            );
        }
        public Task SaveEventAsync(IntegrationEvent @event)
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event);
            //something about transaction
            _eventLogContext.IntegrationEventLogs.Add(eventLogEntry);
            return _eventLogContext.SaveChangesAsync();
        }
        public Task MarkEventAsPublished(Guid evtId)
        {
           var current= _eventLogContext.IntegrationEventLogs.Single(e => e.EventId == evtId);
           current.TimeSent++;
           current.State = EventStateEnum.Published;
           _eventLogContext.IntegrationEventLogs.Update(current);
           return _eventLogContext.SaveChangesAsync();

        }
    }
}
