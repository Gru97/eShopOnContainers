using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands
{
    public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand,bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMediator mediator;
        private readonly IOrderingIntegrationEventService OrderingIntegrationEventService;
        private readonly ILogger<CreateOrderCommandHandler> logger;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMediator mediator,
            IOrderingIntegrationEventService OrderingIntegrationEventService,
            ILogger<CreateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.OrderingIntegrationEventService=OrderingIntegrationEventService ?? throw new ArgumentNullException(nameof(OrderingIntegrationEventService));
            this.logger = logger;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {         

            //Instantiate root aggregate and do necessary things
            var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            var order = new Order(request.UserId, request.UserName, address);
            logger.LogInformation("Order instantiated {@order}",order );
            foreach (var item in request.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.Quantity);
            }

            //(dispatch events and) Persist data
            orderRepository.Add(order);

            await orderRepository.UnitOfWork.SaveChangesAsync();
            logger.LogInformation("Order SaveChanges finished");

            
            return true;
        }

       
    }
}
