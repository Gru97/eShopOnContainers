using BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    //Use in keyword with TIntegrationEvent so we're saying this Interface can work with subclasses of IntegrationEvent
    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent:IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
