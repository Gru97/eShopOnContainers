using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class DoesBuyerExist:IDomainService
    {
        private readonly IBuyerRepository buyerRepository;

        public DoesBuyerExist(IBuyerRepository buyerRepository)
        {
            this.buyerRepository = buyerRepository;
        }

        public async Task<bool> False(string id)
        {
            var _buyer = await buyerRepository.FindAsync(id);
            if (_buyer == null)
                return true;
            return false;
        }
    }
}
