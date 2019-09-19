using BuildingBlocks.EventBus.Events;
using EventBus.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace BuildingBlocks.EventBusRabbitMQ
{
    //Install-Package RabbitMQ.Client -Version 5.1.0
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private string _brokerName="broker";
        private readonly List<Type> eventTypes;
        Dictionary<string, Type> handlers;
        string queueName;
        public EventBusRabbitMQ(string queueName=null)
        {
                eventTypes=new List<Type>();
                handlers=new Dictionary<string, Type>();
                this.queueName = queueName;
        }
        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: _brokerName, type: "direct");
                    string message = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: _brokerName, routingKey: eventName, basicProperties: null, body: body);
                }
            }
            
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {

            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: _brokerName, type: "direct");

                string eventName = typeof(TEvent).Name;
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                    exchange: _brokerName,
                    routingKey: eventName);






                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;

                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                if (!eventTypes.Contains(typeof(TEvent)))
                    eventTypes.Add(typeof(TEvent));
                if (!handlers.ContainsKey(eventName))
                    handlers.Add(eventName, typeof(THandler));
            }
            catch (Exception e)
            {
                throw new Exception("Subscribing to events failed."+e.Message);
            }
            

            
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            Type eventType = eventTypes.Find(t => t.Name == eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body);
            //process event and call the appropriate handler
            Type handlerType = handlers[eventName];
            var integrationEvent= JsonConvert.DeserializeObject(message, eventType);

            //var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
            //await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
            var handler = Activator.CreateInstance(handlerType);
            var x = typeof(IIntegrationEventHandler<>);
            var y=x.MakeGenericType(eventType);
            await (Task)y.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
