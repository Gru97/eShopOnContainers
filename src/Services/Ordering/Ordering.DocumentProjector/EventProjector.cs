using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventStore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.IO;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Events;
using Ordering.QueryModel;

namespace Ordering.DocumentProjector
{
    public interface IEventProjector
    {
        Task Project();
    }
    public class EventProjector:IEventProjector
    {
        //private Dictionary<string, Type> eventMapper;
        private readonly IServiceProvider serviceProvider;
        public IOrderQueries queryStore { get; set; }
        public IDomainEventLogService eventStore { get; set; }

        
        public EventProjector(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public async Task Project()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var eventStore = scope.ServiceProvider.GetRequiredService<IDomainEventLogService>();
                queryStore = serviceProvider.GetRequiredService<IOrderQueries>();

                var unreadEvent = await eventStore
                    .GetEventsAsync(0, 10, e => e.State == EventStateEnum.Unread);
                foreach (var @event in unreadEvent)
                {
                    //Must give it assembly name, otherwise returns null
                    Type type =Type.GetType(@event.EventTypeName+ ", Ordering.Domain");
                    try
                    {
                        dynamic content = Newtonsoft.Json.JsonConvert.DeserializeObject(@event.Content, type);
                        await When(content);
                        await eventStore.MarkEventAsRead(@event.EventId);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                   
                }
            }
        }

        public async Task When(OrderStartedDomainEvent @event)
        {
           
            var doc = new OrderDocument();
            doc.Address = @event.Order.Address.ToAddressDocument();
            doc.OrderItems = @event.Order.OrderItems.ToOrderItemDocumenList();
            doc.OrderId = @event.Order.Id;
            doc.OrderDate = @event.Order.OrderDate;
            doc.Status =@event.Order.OrderState.ToString();
            await queryStore.Upsert(doc);

        }
        public async Task When(BuyerCreatedDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.orderId);
            //if (doc == null)

            doc.BuyerInfo.BuyerId = @event.buyerId.ToString();
            doc.BuyerInfo.BuyerName = @event.buyerName;
            doc.BuyerInfo.BuyerGuid = @event.buyerIdentityGuid;
            await queryStore.Upsert(doc);



        }
        public async Task When(OrderStateChangedToStockConfirmedDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.OrderId);
            if (doc == null)
                throw new Exception("Error while retrieving document");

            doc.Status = OrderState.StockConfirmed.ToString();
            doc.Description = @event.Description;
            await queryStore.Upsert(doc);



        }
        public async Task When(OrderStatusChangedToAwaitingValidationDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.OrderId);
            if (doc == null)
                throw new Exception("Error while retrieving document inside When method of OrderStatusChangedToAwaitingValidationDomainEvent");

            doc.Status = OrderState.AwaitingValidation.ToString();
            await queryStore.Upsert(doc);



        }
        public async Task When(OrderStateChangedToStockRejectedDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.OrderId);
            if (doc == null)
                throw new Exception("Error while retrieving document");
            
            doc.Status =OrderState.Cancelled.ToString();
            doc.Description = @event.Description;

            await queryStore.Upsert(doc);
        }
    }
}
