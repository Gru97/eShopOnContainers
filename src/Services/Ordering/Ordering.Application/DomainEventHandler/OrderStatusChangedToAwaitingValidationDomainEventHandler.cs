using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using IntegrationEventLogEF.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;

namespace Ordering.Application.DomainEventHandler
{
    public class OrderStatusChangedToAwaitingValidationDomainEventHandler
        : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
    {

        private IEventBus _eventBus;
        private IIntegrationEventLogService _integrationEventLogService;
        private ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger;


        public OrderStatusChangedToAwaitingValidationDomainEventHandler(IEventBus eventBus, 
            IIntegrationEventLogService integrationEventLogService,
            ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger)
        {
            this.logger = logger;
            _eventBus = eventBus;
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("OrderStatusChangedToAwaitingValidationDomainEvent handler called");
            var list = new List<StockItem>();
            foreach (var item in notification.OrderItems)
            {
                list.Add(new StockItem() { ProductId =item.ProductId, Units=item.Quantity});
            }
            
            var evt = new OrderStatusChangedToAwaitingValidationIntegrationEvent(list,notification.OrderId);
            logger.LogInformation("OrderStatusChangedToAwaitingValidationIntegrationEvent is created {@evt}",evt);

            //_integrationEventLogService.SaveEventAsync(evt);
            _eventBus.Publish(evt);
            logger.LogInformation("OrderStatusChangedToAwaitingValidationIntegrationEvent is published");

        }
    }
}
