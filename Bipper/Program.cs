using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Bipper
{
    class Program
    {
        static string[] macList = { "AA:AA:AA:AA:AA:AA", "BB:BB:BB:BB:BB:BB", "CC:CC:CC:CC:CC:CC", "CC:2F:71:93:39:F1" };
        static Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        static Random random = new Random();

        static public string GenerateMessage()
        {
            int value = random.Next(10,50);
            string mac = macList[random.Next(4)];
            return "{\"value\": "+ value +", \"macAddress\": \""+ mac +"\", \"date\": "+ unixTimestamp +"}";
        }

        static void LoopMaker()
        {
            var factory = new ConnectionFactory() { HostName = "10.151.129.35" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "receiveData", durable: false, exclusive: false, autoDelete: false, arguments: null);
                while (true)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        string message = GenerateMessage();
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "receiveData-exchange", routingKey: "devices", basicProperties: null, body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                    //Console.WriteLine("500 message sent");
                    Thread.Sleep(1000);
                }
            }     
        }



        static void Main()
        {
            ThreadStart loopStarter = new ThreadStart(LoopMaker);
            Thread looper = new Thread(loopStarter);
            looper.Start();

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
                    looper.Abort();
                }
            }
        }
    }
}
