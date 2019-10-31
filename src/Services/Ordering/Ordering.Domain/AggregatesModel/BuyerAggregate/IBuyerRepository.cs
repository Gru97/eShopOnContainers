using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    // Only the repository contract is defined in Domain layer
    // The implementaion is the Infrastructure layer consern
    public interface IBuyerRepository:IRepository<Buyer>
    {
    }
}
