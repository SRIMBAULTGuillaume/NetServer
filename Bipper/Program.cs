using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Bipper
{
    class Program
    {
        public string GenerateMessage()
        {
            string[] macList = { "AA:AA:AA:AA:AA:AA", "BB:BB:BB:BB:BB:BB", "CC:CC:CC:CC:CC:CC", "CC:2F:71:93:39:F1"};
            DateTime date = DateTime.Now;
            Random random = new Random();
            int value = random.Next(10,50);
            string mac = macList[random.Next(3)];
            string message = "{\"value\": "+ value +", \"macAddress\": "+ mac +", \"date\": "+ date +"}";
            return message;
        }

        static void LoopMaker()
        {
            while(true)
            {
                for(int i=0; i<500; i++)
                {
                    string message = Gene
                }
                Thread.Sleep(1000);
            }
        }



        static void Main()
        {
            string message = GenerateMessage();
            var factory = new ConnectionFactory() { HostName = "10.151.129.35" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "sendData", durable: false, exclusive: false,autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        if (ea.RoutingKey == "AA:AA:AA:AA:AA:AA")
                        {
                            Console.WriteLine(" [x] Received {0}", message);
                            channel.BasicAck(ea.DeliveryTag,false);
                        }
                    };
                    channel.BasicConsume(queue: "sendData", autoAck: false, consumer: consumer);
                    Console.ReadLine();
                }
            }
        }
    }
}
