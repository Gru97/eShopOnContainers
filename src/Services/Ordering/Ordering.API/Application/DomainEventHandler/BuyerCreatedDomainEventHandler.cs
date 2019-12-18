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
            await orderRepository.UnitOfWork.SaveChangesAsync();

        }
    }
}
