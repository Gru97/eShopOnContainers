using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Newtonsoft.Json;

namespace EventStore
{
    public class DomainEventLogEntry
    {
        public Guid EventId { get; set; }
        public string EventTypeName { get; set; }
        [NotMapped]
        public  DomainEvent DomainEvent { get; set; }
        public int TimeSent { get; set; }
        public DateTime CreationTime { get; set; }
        public string Content { get; set; }
        public EventStateEnum State { get; set; }

        public DomainEventLogEntry()
        {
        }

        public DomainEventLogEntry(DomainEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = SafeSerialize(@event);
            State = EventStateEnum.Unread;
            TimeSent = 0;

        }

        private string SafeSerialize(DomainEvent @event)
        {
            try
            {
                return JsonConvert.SerializeObject(@event,Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling. Ignore
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }

        //public DomainEventLogEntry DeserializeJsonContent(Type type)
        //{
        //    DomainEvent = JsonConvert.DeserializeObject(Content, type) as DomainEvent;
        //    return this;
        //}
    }
}

