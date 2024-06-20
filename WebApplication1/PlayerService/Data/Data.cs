using Microsoft.AspNetCore.Connections;
using PlayerService.Models;
using System.Collections.Generic;

namespace PlayerService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task AddPlayerAsync(Player player);
    }

    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "team_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task AddPlayerAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }
    }

    public class TeamCreatedEventConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IPlayerRepository _playerRepository;

        public TeamCreatedEventConsumer(IPlayerRepository playerRepository)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "team_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _playerRepository = playerRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var teamCreatedEvent = JsonConvert.DeserializeObject<TeamCreatedEvent>(message);

                // Handle the event (e.g., update the database, etc.)
            };

            _channel.BasicConsume(queue: "team_created_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}

