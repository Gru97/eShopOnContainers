using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace BuildingBlocks.EventBus.Events
{
    public class IntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; set; }
        [JsonProperty]
        public DateTime CreationDate { get; set; }
        public IntegrationEvent()
        {
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id,DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
