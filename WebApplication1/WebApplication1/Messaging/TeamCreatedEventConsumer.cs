using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using MongoDB.Bson.IO;
namespace PlayerService.Messaging
{
    public class TeamCreatedEventConsumer : BackgroundService
    {
        private readonly IModel _channel;

        public TeamCreatedEventConsumer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "team_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var teamCreatedEvent = JsonConvert.DeserializeObject<TeamCreatedEvent>(message);

                // Handle the event (update the database, etc.)
            };

            _channel.BasicConsume(queue: "team_created_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }

    public class TeamCreatedEvent
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}

