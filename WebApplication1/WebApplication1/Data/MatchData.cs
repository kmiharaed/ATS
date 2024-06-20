using MatchService.Models;
using System.Collections.Generic;

namespace MatchService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task AddMatchAsync(Match match);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;

        public MatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _context.Matches.ToListAsync();
        }

        public async Task AddMatchAsync(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }
    }



