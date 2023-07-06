using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly OrderingContext context;
        public IUnitOfWork UnitOfWork { get { return context; } }

        public BuyerRepository(OrderingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Buyer Add(Buyer buyer)
        {
            return context.Add(buyer).Entity;
        }

        public async Task<Buyer> FindAsync(string buyerIdentityGuid)
        {
            var buyer =await context.Buyers.Where(e=>e.IdentityGuid==buyerIdentityGuid)
                .SingleOrDefaultAsync();
            return buyer;
        }

        public async Task<Buyer> FindByIdAsync(int id)
        {
            var buyer = await context.Buyers.FindAsync(id);
            return buyer;
            


        }

        public void Update(Buyer buyer)
        {
            context.Buyers.Update(buyer);
        }
    }
}
