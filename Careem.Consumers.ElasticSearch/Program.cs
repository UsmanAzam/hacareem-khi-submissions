using Careem.Consumers.ElasticSearch.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careem.Consumers.ElasticSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQConsumer2 client = new RabbitMQConsumer2();
            client.Connect();
            client.ProcessMessages();
           // client.Close();
        }
    }
}
