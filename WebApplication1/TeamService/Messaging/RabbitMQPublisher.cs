using MongoDB.Bson.IO;
using RabbitMQ.Client;
using System.Text;
namespace TeamService.Messaging
{
    public class RabbitMQPublisher
    {
        private readonly IModel _channel;

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "team_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Publish<T>(T message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(exchange: "", routingKey: "team_created_queue", basicProperties: null, body: body);
        }
    }
}

