using Microsoft.AspNetCore.Mvc;
using TeamService.Data;
using TeamService.Models;
namespace TeamService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();
            return Ok(teams);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] Team team)
        {
            await _teamRepository.AddTeamAsync(team);
            return CreatedAtAction(nameof(GetTeams), new { id = team.Id }, team);
        }
    }
}
