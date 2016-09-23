using Infrastructure;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Scheduler.Entities;
using System.Configuration;
using System.Text;

namespace Scheduler
{
    public class Sender
    {
        private readonly ILogger _logger;

        public Sender(ILogger logger)
        {
            _logger = logger;
        }

        public void Send(Message message)
        {
            var hostName = ConfigurationManager.AppSettings["HostName"];
            var userName = ConfigurationManager.AppSettings["UserName"];
            var password = ConfigurationManager.AppSettings["Password"];
            var virtualHost = ConfigurationManager.AppSettings["VirtualHost"];

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new RabbitMessage()
                {
                    Destination = message.Destination,
                    Source = message.Source,
                    Text = message.Text
                }));

                channel.QueueDeclare(queue: message.Type.ToString(),
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                channel.BasicPublish(exchange: "",
                                     routingKey: message.Type.ToString(),
                                     basicProperties: null,
                                     body: body);

                //channel.ExchangeDeclare(exchange: "messages", type: "direct");

                

                //channel.BasicPublish(exchange: "MyEx",
                //                     routingKey: message.Type.ToString(),
                //                     basicProperties: null,
                //                     body: body);
                _logger.Info($"Sent {message}");
            }
        }

        private class RabbitMessage
        {
            public string Destination { get; set; }

            public string Source { get; set; }

            public string Text { get; set; }
        }
    }
}