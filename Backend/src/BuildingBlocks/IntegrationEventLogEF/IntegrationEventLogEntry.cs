using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BuildingBlocks.EventBus.Events;
using Newtonsoft.Json;

namespace IntegrationEventLogEF
{
    public class IntegrationEventLogEntry
    {
        public Guid EventId { get; set; }
        public string EventTypeName { get; set; }
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; set; }
        public int TimeSent { get; set; }
        public DateTime CreationTime { get; set; }
        public string Content { get; set; }
        public EventStateEnum State { get; set; }

        public IntegrationEventLogEntry()
        {
        }

        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimeSent = 0;

        }

        //public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        //{
        //    IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
        //    return this;
        //}
    }

}
