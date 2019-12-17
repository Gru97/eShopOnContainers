using MediatR;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.DomainEventHandler
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {

        private readonly IBuyerRepository buyerRepository;

        public async Task Handle(OrderStartedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Side effects of creating an order is to create a buyer (if it does not exist)

            var buyer = await buyerRepository.FindAsync(@event.UserId);
            if (buyer == null)
            {
                buyer = new Buyer(@event.UserName, @event.UserId, @event.Order.Id);
                await buyerRepository.UnitOfWork.SaveChangesAsync();
            }

            
        }

       
    }
}
