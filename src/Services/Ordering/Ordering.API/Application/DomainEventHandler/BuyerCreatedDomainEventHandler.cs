using MediatR;
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

        public BuyerCreatedDomainEventHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task Handle(BuyerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindAsycn(notification.orderId);
            order.BuyerId = notification.buyerId;
            order.SetAwaitingValidationStatus();
            //This transaction hasn't commited yet (We are before executing the previous SaveChanges())
            //So no need to call SaveChanges again
            //await orderRepository.UnitOfWork.SaveChangesAsync();

        }
    }
}
