using EventBus.Abstractions;
using IntegrationEventLogEF.Services;
using MediatR;
using Ordering.API.Application.IntegrationEvents.Events;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.DomainEventHandler
{
    public class OrderStatusChangedToAwaitingValidationDomainEventHandler
        : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
    {

        private IEventBus _eventBus;
        private IIntegrationEventLogService _integrationEventLogService;

        public OrderStatusChangedToAwaitingValidationDomainEventHandler(IEventBus eventBus, 
            IIntegrationEventLogService integrationEventLogService)
        {
            _eventBus = eventBus;
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
        {
            var list = new List<StockItem>();
            foreach (var item in notification.OrderItems)
            {
                list.Add(new StockItem() { ProductId =item.ProductId, Units=item.Quantity});
            }
            
            var evt = new OrderStatusChangedToAwaitingValidationIntegrationEvent(list,notification.OrderId);
            //_integrationEventLogService.SaveEventAsync(evt);
            _eventBus.Publish(evt);
            
        }
    }
}
