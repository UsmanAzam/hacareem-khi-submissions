using Careem.Common.Models.Rides;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Careem.Common.Extensions;

namespace Careem.Consumers.SqlServer.RabbitMQ
{
    internal class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string ExchangeName = "Topic_Exchange";
        private const string SqlServerQueueName = "SqlServerTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            _factory.AutomaticRecoveryEnabled = true;
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <ride.sqlserver>");
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine();

                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(SqlServerQueueName, true, false, false, null);
                    channel.QueueBind(SqlServerQueueName, ExchangeName, "ride.sqlserver");

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, SqlServerQueueName, false);

                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();

                        var message = (Ride)deliveryArguments.Body.DeSerialize(typeof(Ride));
                        var routingKey = deliveryArguments.RoutingKey;

                        // Console.WriteLine("-- New Ride - Routing Key <{0}> : rideId: {1} userId {2}", routingKey, message.userId, message.rideId);
                        subscription.Ack(deliveryArguments);
                    }



                }
            }
        }
    }
}
