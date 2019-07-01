using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;



namespace Bipper
{
    class Program
    {
        static string[] macList = { "c0:e4:bf:30:3a:c3", "d4:36:2f:bb:16:48", "8c:26:2c:0b:bc:f5", "48:22:ae:c8:0a:bf", "3c:ae:8d:9e:69:2e", "c0:63:5d:c8:e8:34", "4c:a6:6f:0a:1b:ef", "02:0f:83:d2:b2:a5" };
        static bool led = false;

        

        static Random random = new Random();

        static public string GenerateMessage(int device)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            int value;
            if (device == 5)
                value = led ? 50 : 0;
            else
                value = random.Next(10, 50);

            string mac = macList[device];
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
                    for (int i = 0; i < 8; i++)
                    {
                        string message = GenerateMessage(i);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "receiveData-exchange", routingKey: "devices", basicProperties: null, body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                    Console.WriteLine("-----------------------------------------------------------------------------------------");
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
                        if (ea.RoutingKey == "c0:63:5d:c8:e8:34")
                            led = !led;
                        Console.WriteLine(" [x] Received {0}, Device Mac : {1}", message, ea.RoutingKey);
                        channel.BasicAck(ea.DeliveryTag,false);
                    };
                    channel.BasicConsume(queue: "sendData", autoAck: false, consumer: consumer);
                    Console.ReadLine();
                }
            }
            looper.Abort();
        }
    }
}
