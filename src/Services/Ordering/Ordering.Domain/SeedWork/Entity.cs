using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.SeedWork
{
    public abstract class Entity
    {
        private int id;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }
        public override bool Equals(object obj)
        {
            //TODO Implement this
            return base.Equals(obj);
        }


        //Each entity has a list of domain events. These events are added to the list instead of being raised in the entity method.
        //They will be raised later in application layer
        private List<INotification> domainEvents;

        public IReadOnlyCollection<INotification> DomainEvents
        {
            get { return domainEvents; }
            private set { }
        }

        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents = domainEvents ?? new List<INotification>();
            domainEvents.Add(eventItem);

        }
        public void RemoveDomainEvent(INotification eventItem)
        {
            domainEvents.Remove(eventItem);
        }
        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }




    }
}
