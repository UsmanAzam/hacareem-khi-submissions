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
using System.Diagnostics;


namespace Careem.Consumers.ElasticSearch.RabbitMQ
{
    internal class RabbitMQConsumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string ExchangeName = "Topic_Exchange";
        private const string ElasticSearchQueueName = "ElasticSearchTopic_Queue";

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
            try
            {
                using (_connection = _factory.CreateConnection())
                {
                    using (var channel = _connection.CreateModel())
                    {
                        Console.WriteLine("Listening for Topic <ride.elasticsearch>");
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine();

                        channel.ExchangeDeclare(ExchangeName, "topic");
                        channel.QueueDeclare(ElasticSearchQueueName, true, false, false, null);
                        channel.QueueBind(ElasticSearchQueueName, ExchangeName, "ride.elasticsearch");

                        channel.BasicQos(0, 10, false);
                        Subscription subscription = new Subscription(channel, ElasticSearchQueueName, false);

                        while (true)
                        {
                            BasicDeliverEventArgs deliveryArguments = subscription.Next();

                            var message = (Ride)deliveryArguments.Body.DeSerialize(typeof(Ride));
                            var routingKey = deliveryArguments.RoutingKey;

                            //Console.WriteLine("-- New Ride - Routing Key <{0}> : rideId: {1} userId {2}", routingKey, message.userId, message.rideId);
                            subscription.Ack(deliveryArguments);


                        }


         

                    }
                }
            }//try
            catch(Exception e )
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
