using Microsoft.AspNetCore.Connections;
using System.Collections.Generic;
using System.Text;
using TeamService.Models;
namespace TeamService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task AddTeamAsync(Team team);
    }

    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "team_created_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task AddTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var teamCreatedEvent = new
            {
                TeamId = team.Id,
                Name = team.Name,
                City = team.City
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(teamCreatedEvent));
            _channel.BasicPublish(exchange: "", routingKey: "team_created_queue", basicProperties: null, body: body);
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }
    }
}
