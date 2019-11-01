using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    // Only the repository contract is defined in Domain layer
    // The implementaion is the Infrastructure layer consern
    public interface IBuyerRepository:IRepository<Buyer>
    {
        Buyer Add(Buyer buyer);
        void Update(Buyer buyer);
        Task<Buyer> FindAsync(string buyerIdentityGuid);
        Task<Buyer> FindByIdAsync(int id);
    }
}
