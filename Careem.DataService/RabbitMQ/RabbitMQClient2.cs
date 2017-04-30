using Careem.Common.Models.Rides;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Careem.DataService.Extensions;
using System.Web;

namespace Careem.DataService.RabbitMQ
{
    public class RabbitMQClient2
    {
        private IConnection connection;
        private IModel channel;

        private const string ExchangeName = "PubSubTestExchange";

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
        }

        public void Close()
        {
            channel = null;

            if (connection.IsOpen)
            {
                connection.Close();
            }

            connection.Dispose();
            connection = null;
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
            channel.BasicPublish(ExchangeName, string.Empty, null, message);
            //_model.BasicPublish(ExchangeName, routingKey, null, message);
        }

    }
}