using System.Threading.Tasks;
using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.IntegrationEvents.Events;

namespace Ordering.Application.IntegrationEvents.EventHandling
{
    public class OrderStockConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
    {
        private readonly IMediator mediator;
        private ILogger<OrderStockConfirmedIntegrationEventHandler> logger;

        public OrderStockConfirmedIntegrationEventHandler(IMediator mediator,
            ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
        {
            var cmd = new SetOrderStatusToStockConfirmedCommand(@event.orderId);
            logger.LogInformation("SetOrderStatusToStockConfirmedCommand created:{@cmd} ",cmd);
            await mediator.Send(cmd);
            logger.LogInformation("SetOrderStatusToStockConfirmedCommand sent ");


        }
    }
}
