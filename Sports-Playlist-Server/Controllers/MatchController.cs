using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Enums;
using Sports_Playlist_Server.Extensions;
using Sports_Playlist_Server.Interfaces;
using Sports_Playlist_Server.Mappers;
using Sports_Playlist_Server.Models;
using Sports_Playlist_Server.Validators;

namespace Sports_Playlist_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;
        private readonly UserManager<User> _userManager;

        public MatchesController(IMatchRepository matchRepository, UserManager<User> userManager)
        {
            _matchRepository = matchRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            try
            {
                var matches = await _matchRepository.GetAllAsync();
                return Ok(matches.FromMatchesToMatchesDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            try
            {
                var match = await _matchRepository.GetMatchWithDetails(id);
                return Ok(match);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Match with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromBody] CreateMatchDto matchDto)
        {
            var validator = new MatchCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(matchDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var match = matchDto.FromCreateMatchDtoToMatch();
                await _matchRepository.AddAsync(match);
                return CreatedAtAction(nameof(GetMatchById), new { id = match.Id }, match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] UpdateMatchDto updatedMatch)
        {
            var validator = new MatchUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(updatedMatch);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var match = await _matchRepository.GetByIdAsync(id);

                match.Title = updatedMatch.Title;
                match.Competition = updatedMatch.Competition;
                match.MatchDate = updatedMatch.MatchDate;
                match.Status = updatedMatch.Status;

                await _matchRepository.UpdateAsync(match);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Match with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            try
            {
                await _matchRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Match with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetMatchesByStatus([FromQuery] string status)
        {
            try
            {
                if (!Enum.TryParse<MatchStatus>(status, true, out var parsedStatus))
                {
                    return BadRequest("Invalid status value.");
                }

                var matches = await _matchRepository.GetMatchesByStatus(parsedStatus);

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("userPlaylist")]
        public async Task<IActionResult> GetUserPlayListMatches()
        {
            try
            {
                var currentLoggedInUserId = HttpContext.User.GetUserId();

                var currentLoggedInUser = await _userManager.Users
                    .SingleOrDefaultAsync(x => x.Id == currentLoggedInUserId.ToString());

                if (currentLoggedInUser == null)
                {
                    return NotFound("User not found.");
                }

                var matches = await _matchRepository.GetUserMatchesFromPlaylist(currentLoggedInUserId.ToString());

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}