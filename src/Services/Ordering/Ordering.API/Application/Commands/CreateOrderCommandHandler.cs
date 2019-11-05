using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMediator mediator;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(CreateOrderCommand message)
        {
            //TODO: Create and raise an Integration event: OrderStartedIntegrationEvent

            //Instantiate root aggregate and do necessary things
            var address = new Address(message.Street, message.City, message.State, message.Country, message.ZipCode);
            var order = new Order(message.UserId, message.UserName, address);
            foreach (var item in message.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.Quantity);
            }

            //Persist data
            orderRepository.Add(order);
            await orderRepository.UnitOfWork.SaveChangesAsync();

        }
    }
}
