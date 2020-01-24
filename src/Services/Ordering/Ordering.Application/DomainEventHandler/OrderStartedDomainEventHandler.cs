using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandler
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {

        private readonly IBuyerRepository buyerRepository;
        private readonly DoesBuyerExist doesBuyerExistService;
        private readonly ILogger<OrderStartedDomainEventHandler> logger;

        public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository, 
            DoesBuyerExist doesBuyerExistService,
            ILogger<OrderStartedDomainEventHandler> logger)
        {
            this.logger = logger;
            this.buyerRepository = buyerRepository;
            this.doesBuyerExistService = doesBuyerExistService;
        }

        public async Task Handle(OrderStartedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Side effects of creating an order is to create a buyer (if it does not exist)
            //var buyer = new BuyerInfo(@event.UserName, @event.UserId, @event.Order.Id, doesBuyerExistService);
            //buyer=buyerRepository.Upsert(buyer);

            var buyer = await buyerRepository.FindAsync(@event.UserId);

            if (buyer == null)
            {
                logger.LogInformation("buyer didn't exist");
                buyer = new Buyer(@event.UserName, @event.UserId, @event.Order.Id);
                logger.LogInformation("buyer instantiated");

                buyer = buyerRepository.Add(buyer);

            }


            //Probably the wrong way
            buyer.AddDomainEvent(new BuyerCreatedDomainEvent(@event.Order.Id, buyer.Id,buyer.Name,buyer.IdentityGuid));
            logger.LogInformation("BuyerCreatedDomainEvent added to list of buyer events");

            await buyerRepository.UnitOfWork.SaveChangesAsync();
            logger.LogInformation("SaveChanges called on buyer");

        }


    }
}
