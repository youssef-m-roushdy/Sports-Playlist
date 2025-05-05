using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sports_Playlist_Server.Extensions;
using Sports_Playlist_Server.Interfaces;

namespace Sports_Playlist_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMatchToUserPlaylist([FromBody] int matchId)
        {
            var currentLoggedInUserId = HttpContext.User.GetUserId();

            try
            {
                await _playlistRepository.AddPlaylistToUser(currentLoggedInUserId.ToString(), matchId);
                return Ok("Match successfully added to playlist.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMatchFromUserPlaylist([FromRoute]int id)
        {
            try
            {
                await _playlistRepository.DeleteMatchFromPlayList(id);
                return Ok("Match successfully removed from playlist.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}