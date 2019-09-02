using BuildingBlocks.EventBus.Events;
using EventBus.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.EventBusRabbitMQ
{
    //Install-Package RabbitMQ.Client -Version 5.1.0
    public class EventBusRabbitMQ : IEventBus
    {
        private string _brokerName="broker";

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
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _brokerName, type: "direct");

                string eventName = typeof(TEvent).Name;
                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: _brokerName,
                                  routingKey: eventName);

                

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
               
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body);
            //process event and call the appropriate handler
            

           
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }
    }
}
