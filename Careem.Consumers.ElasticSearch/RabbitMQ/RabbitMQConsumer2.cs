using Careem.Common.Models.Rides;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Careem.Common.Extensions;

using System.Threading.Tasks;

namespace Careem.Consumers.ElasticSearch.RabbitMQ
{
    internal class RabbitMQConsumer2
    {
        private IConnection connection;
        private IModel channel;

        private const string ExchangeName = "PubSubTestExchange";

        private string queueName;

        public void Connect()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, "fanout");

            // queue name is generated
            queueName = channel.QueueDeclare();
            channel.QueueBind(queueName, ExchangeName, string.Empty);
        }

        public void ProcessMessages()
        {
            QueueingBasicConsumer consumer = MakeConsumer();

            bool done = false;
            while (!done)
            {
                ReadAMessage(consumer);
            }

            connection.Close();
            connection.Dispose();
            connection = null;
        }

      

        private QueueingBasicConsumer MakeConsumer()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);
            return consumer;
        }

        private static void ReadAMessage(QueueingBasicConsumer consumer)
        {
            BasicDeliverEventArgs delivery = DequeueMessage(consumer);
            if (delivery == null)
            {
                return;
            }

            try
            {
                var message = (Ride)delivery.Body.DeSerialize(typeof(Ride));
                Console.WriteLine("Received {0} : {1}", message.GetType().Name, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed message: {0}", ex);
            }
        }

        private static BasicDeliverEventArgs DequeueMessage(QueueingBasicConsumer consumer)
        {
            const int timeoutMilseconds = 400;
            BasicDeliverEventArgs result;

            consumer.Queue.Dequeue(timeoutMilseconds, out result);
            return result ;
        }
    }
}
