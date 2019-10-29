using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class Buyer:Entity,IAggregateRoot
    {
        public string Name { get; private set; }


    }
}
