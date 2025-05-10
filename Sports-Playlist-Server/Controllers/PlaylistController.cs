using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Extensions;
using Sports_Playlist_Server.Interfaces;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly UserManager<User> _userManager;

        public PlaylistController(IPlaylistRepository playlistRepository, UserManager<User> userManager)
        {
            _playlistRepository = playlistRepository;
                        _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddMatchToUserPlaylist([FromBody] int matchId)
        {
            var currentLoggedInUserId = HttpContext.User.GetUserId();
            var currentLoggedInUser = await _userManager.Users
                    .SingleOrDefaultAsync(x => x.Id == currentLoggedInUserId.ToString());

            if (currentLoggedInUser == null)
            {
                return NotFound(new { Error = "Can't add match to undefined user playlist." });
            }

            try
            {
                await _playlistRepository.AddPlaylistToUser(currentLoggedInUserId.ToString(), matchId);
                return Ok(new { Message = "Match successfully added to playlist." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpDelete("{matchId}")]
        public async Task<IActionResult> DeleteMatchFromUserPlaylist(int matchId)
        {
            var currentLoggedInUserId = HttpContext.User.GetUserId();
            var currentLoggedInUser = await _userManager.Users
                    .SingleOrDefaultAsync(x => x.Id == currentLoggedInUserId.ToString());

            if (currentLoggedInUser == null)
            {
                return NotFound(new { Error = "Can't delete match to undefined user playlist." });
            }

            try
            {
                await _playlistRepository.DeleteMatchFromPlayList(currentLoggedInUserId.ToString(), matchId);
                return Ok(new { Message = "Match successfully removed from playlist." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred: " + ex.Message });
            }
        }
    }
}