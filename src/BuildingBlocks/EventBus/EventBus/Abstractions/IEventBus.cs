using BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
        void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;
        void Unsubscribe<TEvent, THandler>()
        where TEvent : IntegrationEvent
        where THandler : IIntegrationEventHandler<TEvent>;

    }
}
