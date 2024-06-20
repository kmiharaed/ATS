using TeamService.Messaging;
using TeamService.Models;
namespace TeamService.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQPublisher _publisher;

        public TeamRepository(AppDbContext context, RabbitMQPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
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

            _publisher.Publish(teamCreatedEvent);
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }
    }
}
