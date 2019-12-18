using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class Buyer:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public string IdentityGuid { get; private set; }
        public Buyer()
        {

        }

        public Buyer(string name, string identityGuid,int orderId,DoesBuyerExist doesBuyerExist)
        {
            if (doesBuyerExist.False(identityGuid).Result)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    throw new ArgumentNullException();
                else
                    Name = name;
                if (string.IsNullOrWhiteSpace(identityGuid))
                    throw new ArgumentNullException();
                else
                    IdentityGuid = identityGuid;
            }

            AddDomainEvent(new BuyerCreatedDomainEvent(orderId,this.Id));
          

        }
    }
}
