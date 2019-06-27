using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Bipper
{
    class Program
    {
        static void Main()
        {
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
