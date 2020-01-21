using BuildingBlocks.EventBus.Events;
using EventBus.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace BuildingBlocks.EventBusRabbitMQ
{
    //Install-Package RabbitMQ.Client -Version 5.1.0
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private string _brokerName="broker";
        private readonly List<Type> eventTypes;
        Dictionary<string, Type> handlers;
        string queueName;
        IServiceProvider serviceProvider;
        public EventBusRabbitMQ(IServiceProvider serviceProvider, string queueName=null)
        {
                eventTypes=new List<Type>();
                handlers=new Dictionary<string, Type>();
                this.serviceProvider = serviceProvider;
                this.queueName = queueName;
        }
        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;
            var factory = new ConnectionFactory() { HostName = "rabbitmq"};
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: _brokerName, type: "direct");
                    string message = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(message);
                    try
                    {
                        channel.BasicPublish(exchange: _brokerName, routingKey: eventName, basicProperties: null, body: body);

                    }
                    catch (Exception ex)
                    {

                        throw new Exception (ex.Message);
                    }
                }
            }
            
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {

            try
            {
                Thread.Sleep(20000);
                var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672,DispatchConsumersAsync = true };
                Thread.Sleep(20000);

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
                Console.WriteLine("Subscribing to events failed." + e.Message);
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


            object handler=null;

            //It was before using ActivatorUtilities, we had to find ctor and their params and use serviceProvider to instantiate them
            //ConstructorInfo constructor = handlerType.GetConstructors()[0];

            //if (constructor != null)
            //{
            //    object[] args = constructor
            //        .GetParameters()
            //        .Select(o => o.ParameterType)
            //        .Select(o => serviceProvider.GetService(o))
            //        .ToArray();

            //    handler=Activator.CreateInstance(handlerType, args);
            //}
            //else
            //    handler = Activator.CreateInstance(handlerType);

            //var handler = serviceProvider.GetService(handlerType);

            //*Scope Problem
            using (var scope = serviceProvider.CreateScope())
            {
                //using ActivatorUtilities is possible in dotnet core. Just does the same foreach above
                handler = ActivatorUtilities.CreateInstance(scope.ServiceProvider, handlerType);
                var x = typeof(IIntegrationEventHandler<>);
                var y = x.MakeGenericType(eventType);
                await (Task)y.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

            }
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
