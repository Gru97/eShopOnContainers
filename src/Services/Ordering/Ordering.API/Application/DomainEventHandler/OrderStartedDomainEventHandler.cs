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
        private readonly DoesBuyerExist doesBuyerExistService;

        public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository, DoesBuyerExist doesBuyerExistService)
        {
            this.buyerRepository = buyerRepository;
            this.doesBuyerExistService = doesBuyerExistService;
        }

        public async Task Handle(OrderStartedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Side effects of creating an order is to create a buyer (if it does not exist)
            //var buyer = new Buyer(@event.UserName, @event.UserId, @event.Order.Id, doesBuyerExistService);
            //buyer=buyerRepository.Add(buyer);



            var buyer = await buyerRepository.FindAsync(@event.UserId);
            if (buyer == null)
            {
                buyer = new Buyer(@event.UserName, @event.UserId, @event.Order.Id);
                buyer=buyerRepository.Add(buyer);

            }


            //Probably the wrong way
            buyer.AddDomainEvent(new BuyerCreatedDomainEvent(@event.Order.Id, buyer.Id));
            await buyerRepository.UnitOfWork.SaveChangesAsync();

        }


    }
}
