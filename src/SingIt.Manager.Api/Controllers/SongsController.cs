using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SingIt.Manager.Api.Models;
using SingIt.Manager.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SingIt.Manager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SongsController : ControllerBase
    {
        private readonly SongService _songService;

        public SongsController(SongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<IEnumerable<Song>> All()
            => await _songService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> Get(string id)
        {
            var song = await _songService.GetAsync(id);

            if (song == null)
            {
                return NotFound(id);
            }

            return Ok(song);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Song song)
        {
            var created = await _songService.CreateAsync(song);

            if (created)
            {
                return CreatedAtAction(nameof(Get), new { id = song.Id }, song);
            }

            return Ok(song);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(IEnumerable<Song> songs, CancellationToken cancellationToken)
        {
            if (!songs.Any())
            {
                return BadRequest("Missing the songs.");
            }

            await _songService.RefreshAsync(songs, cancellationToken);
            return Ok();
        }
    }
}
