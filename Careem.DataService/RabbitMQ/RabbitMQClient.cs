using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Careem.DataService.Extensions;
using Careem.Common.Models.Rides;
using System.Diagnostics;

namespace Careem.DataService.RabbitMQ
{
    public class RabbitMQClient
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string SqlServerQueueName = "SqlServerTopic_Queue";
        private const string ElasticSearchQueueName = "ElasticSearchTopic_Queue";

        public RabbitMQClient()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(SqlServerQueueName, true, false, false, null);
            _model.QueueDeclare(ElasticSearchQueueName, true, false, false, null);
           

            _model.QueueBind(SqlServerQueueName, ExchangeName, "ride.sqlserver");
            _model.QueueBind(ElasticSearchQueueName, ExchangeName, "ride.elasticsearch");
           
        }

        public void Close()
        {
            _connection.Close();
        }

        public void SaveRide(Ride ride)
        {
            SendMessage(ride.Serialize(), "ride.sqlserver");
            
            //Debug.WriteLine(" Ride Sent {0}, of user {1}", ride.rideId,
            //    ride.userId);

            SendMessage(ride.Serialize(), "ride.elasticsearch");


            //Debug.WriteLine(" Ride Sent {0}, of user {1}", ride.rideId,
            //   ride.userId);

        }

        

        public void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(ExchangeName, routingKey, null, message);
        }
    }
}