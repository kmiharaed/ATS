using Microsoft.AspNetCore.Mvc;
using PlayerService.Data;
using PlayerService.Models;

namespace PlayerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _playerRepository.GetAllPlayersAsync();
            return Ok(players);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] Player player)
        {
            await _playerRepository.AddPlayerAsync(player);
            return CreatedAtAction(nameof(GetPlayers), new { id = player.Id }, player);
        }
    }
}
