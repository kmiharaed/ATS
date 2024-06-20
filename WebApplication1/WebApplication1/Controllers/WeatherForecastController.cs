using MatchService.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;

        public MatchesController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var matches = await _matchRepository.GetAllMatchesAsync();
            return Ok(matches);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromBody] Match match)
        {
            await _matchRepository.AddMatchAsync(match);
            return CreatedAtAction(nameof(GetMatches), new { id = match.Id }, match);
        }
    }

}