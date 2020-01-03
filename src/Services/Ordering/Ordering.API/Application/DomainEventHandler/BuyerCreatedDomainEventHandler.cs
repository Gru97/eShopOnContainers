using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.DomainEventHandler
{
    public class BuyerCreatedDomainEventHandler : INotificationHandler<BuyerCreatedDomainEvent>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger <BuyerCreatedDomainEventHandler> logger;
        public BuyerCreatedDomainEventHandler(IOrderRepository orderRepository,
            ILogger<BuyerCreatedDomainEventHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
        }

        public async Task Handle(BuyerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindAsycn(notification.orderId);
            logger.LogInformation($"order retrieved to set its buyer. {@order}");

            order.BuyerId = notification.buyerId;
            logger.LogInformation($"buyerId set to {@order.BuyerId}");

            order.SetAwaitingValidationStatus();
            logger.LogInformation("OrderStatus is set to SetAwaitingValidationStatus ");

            //This transaction hasn't commited yet (We are before executing the previous SaveChanges())
            //So no need to call SaveChanges again
            //await orderRepository.UnitOfWork.SaveChangesAsync();

        }
    }
}
