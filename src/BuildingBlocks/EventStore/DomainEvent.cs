using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;


namespace EventStore
{
    public class DomainEvent:INotification
    {
        [JsonProperty]
        public Guid Id { get; set; }
        [JsonProperty]
        public DateTime CreationDate { get; set; }
        public DomainEvent()
        {
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public DomainEvent(Guid id,DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
