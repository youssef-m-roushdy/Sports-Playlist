using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Enums;
using Sports_Playlist_Server.Interfaces;
using Sports_Playlist_Server.Mappers;
using Sports_Playlist_Server.Models;
using Sports_Playlist_Server.Validators;

namespace Sports_Playlist_Server.Controllers
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
            if (!Enum.TryParse<MatchStatus>(status, true, out var parsedStatus))
            {
                return BadRequest("Invalid status value.");
            }

            var matches = await _matchRepository.GetMatchesByStatus(parsedStatus);

            return Ok(matches);
        }
    }
}