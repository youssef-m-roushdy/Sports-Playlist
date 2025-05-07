using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Extensions;
using Sports_Playlist_Server.Interfaces;

namespace Sports_Playlist_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddMatchToUserPlaylist([FromBody] int matchId)
        {
            var currentLoggedInUserId = HttpContext.User.GetUserId();

            try
            {
                await _playlistRepository.AddPlaylistToUser(currentLoggedInUserId.ToString(), matchId);
                return Ok(new {Message = "Match successfully added to playlist."});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new {Error = ex.Message});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new {Error = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {Error = "An unexpected error occurred: " + ex.Message});
            }
        }

        [HttpDelete("{matchId}")]
        public async Task<IActionResult> DeleteMatchFromUserPlaylist(int matchId)
        {
            var currentLoggedInUserId = HttpContext.User.GetUserId();

            try
            {
                await _playlistRepository.DeleteMatchFromPlayList(currentLoggedInUserId.ToString(), matchId);
                return Ok(new {Message = "Match successfully removed from playlist."});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new {Error = ex.Message});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new {Error = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500,new {Error = "An unexpected error occurred: " + ex.Message});
            }
        }
    }
}