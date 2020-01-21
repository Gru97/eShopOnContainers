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

        //private static string OrderStarted = "Ordering.Domain.Events.OrderStartedDomainEvent";
        //private static string BuyerCreated = "Ordering.Domain.AggregatesModel.BuyerAggregate.BuyerCreatedDomainEvent";
        //private static string OrderStatusAwaitingValidation = "Ordering.Domain.Events.OrderStatusChangedToAwaitingValidationDomainEvent";
        //private static string OrderStateStockConfirmed = "Ordering.Domain.Events.OrderStateChangedToStockConfirmedDomainEvent";
        //private static string OrderStateRejected = "Ordering.Domain.Events.OrderStateChangedToStockRejectedDomainEvent";
        public IOrderQueries queryStore { get; set; }
        public IDomainEventLogService eventStore { get; set; }

        //public EventProjector(IOrderQueries queryStore, IDomainEventLogService eventStore )
        //{
        //    this.queryStore = queryStore; 
        //    //this.eventStore = eventStore;
        //    //eventMapper.Add(OrderStarted, typeof(Order));
        //    //eventMapper.Add(BuyerCreated, typeof(Order));
        //    //eventMapper.Add(OrderStatusAwaitingValidation, typeof(Buyer));
        //}

        public EventProjector(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            //queryStore = serviceProvider.GetRequiredService<IOrderQueries>();
            //eventStore = serviceProvider.GetRequiredService<IDomainEventLogService>();
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
                        //var x = Convert.ChangeType(content, eventType);
                        await When(content);
                        //mongoRepository.GetOrderAsync();
                        //await eventStore.MarkEventAsRead(@event.EventId);

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
            //var doc = await queryStore.GetOrderDocument(@event.Order.Id);
            //var doc = await queryStore.GetOrderDocument(@event.Order.Id);
            //if(doc==null)
            var doc = new OrderDocument();
            //doc.BuyerInfo=@event.Order.
            doc.Address = @event.Order.Address.ToAddressDocument();
            doc.OrderItems = @event.Order.OrderItems.ToOrderItemDocumenList();
            doc.OrderId = @event.Order.Id;
            doc.OrderDate = @event.Order.OrderDate;
            doc.Status =(short) @event.Order.OrderState;
            await queryStore.Upsert(doc);

        }
        public async Task When(BuyerCreatedDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.orderId);
            //if (doc == null)

            doc.BuyerInfo.BuyerId = @event.buyerId.ToString();
            //doc.BuyerInfo.BuyerName = @event.buyerId.ToString();  ??
            await queryStore.Upsert(doc);



        }
        public async Task When(OrderStateChangedToStockConfirmedDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.OrderId);
            if (doc == null)
                throw new Exception("Error while retrieving document");
            //doc.BuyerInfo.BuyerId = @event.ToString();
            doc.Status = (short)OrderState.StockConfirmed;
            await queryStore.Upsert(doc);



        }
        public async Task When(OrderStatusChangedToAwaitingValidationDomainEvent @event)
        {
            var doc = await queryStore.GetOrderDocument(@event.OrderId);
            if (doc == null)
                throw new Exception("Error while retrieving document");
            //doc.BuyerInfo.BuyerId = @event.ToString();
            doc.Status = (short)OrderState.AwaitingValidation;
            await queryStore.Upsert(doc);



        }
        //public async Task When(OrderStateChangedToStockRejectedDomainEvent @event)
        //{
        //    var doc = await queryStore.GetOrderDocument(@event.OrderId);
        //    if (doc == null)
        //        throw new Exception("Error while retrieving document");
        //    doc.BuyerInfo.BuyerId = @event.ToString();
        //    doc.Status = (short)OrderState.AwaitingValidation;
        //    await queryStore.Upsert(doc);



        //}
    }
}
