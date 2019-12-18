using MediatR;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand,bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMediator mediator;
        private readonly IOrderingIntegrationEventService OrderingIntegrationEventService;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator, IOrderingIntegrationEventService OrderingIntegrationEventService)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.OrderingIntegrationEventService=OrderingIntegrationEventService ?? throw new ArgumentNullException(nameof(OrderingIntegrationEventService));
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {         

            //Instantiate root aggregate and do necessary things
            var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            var order = new Order(request.UserId, request.UserName, address);
            foreach (var item in request.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.Quantity);
            }

            //(dispatch events and) Persist data
            orderRepository.Add(order);
            await orderRepository.UnitOfWork.SaveChangesAsync();
            
            return true;
        }

       
    }
}
