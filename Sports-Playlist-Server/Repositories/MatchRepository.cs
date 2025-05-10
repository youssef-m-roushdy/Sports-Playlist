using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.Data;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Enums;
using Sports_Playlist_Server.Interfaces;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;
        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchDto>> GetAllAsync()
        {
            return await _context.Matches.AsNoTracking().Select(m => new MatchDto
            {
                Id = m.Id,
                Title = m.Title,
                Competition = m.Competition,
                MatchDate = m.MatchDate,
                Status = m.Status
            }).ToListAsync();
        }

        public async Task<MatchDetailsDto> GetByIdWithDetails(int matchId)
        {
            var matches = await _context.Matches
                .Where(m => m.Id == matchId)
                .Select(m => new MatchDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Competition = m.Competition,
                    MatchDate = m.MatchDate,
                    MatchUrl = m.MatchUrl,
                    Status = m.Status,
                }).FirstOrDefaultAsync();

            if (matches == null)
            {
                throw new KeyNotFoundException($"Match with id {matchId} not found.");
            }

            return matches;
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesByStatus(MatchStatus status)
        {
            return await _context.Matches
                          .Where(m => m.Status == status)
                          .Select(p => new MatchDto
                          {
                              Id = p.Id,
                              Title = p.Title,
                              Competition = p.Competition,
                              MatchDate = p.MatchDate,
                              Status = p.Status
                          })
                          .ToListAsync();
        }
        public async Task<IEnumerable<MatchDto>> GetMatchesByCompetition(string competition)
        {
            return await _context.Matches
                .Where(m => m.Competition == competition)
                .Select(p => new MatchDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Competition = p.Competition,
                    MatchDate = p.MatchDate,
                    Status = p.Status
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchDto>> GetUserMatchesFromPlaylist(string userId)
        {
            return await _context.Playlists
                .Include(p => p.Match)
                .Where(p => p.UserId == userId)
                .Select(p => new MatchDto
                {
                    Id = p.Match.Id,
                    Title = p.Match.Title,
                    Competition = p.Match.Competition,
                    MatchDate = p.Match.MatchDate,
                    Status = p.Match.Status
                })
                .ToListAsync();
        }

        public async Task AddAsync(Match entity)
        {
            await _context.Matches.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateMatchDto matchDto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                throw new KeyNotFoundException($"Match with id {id} not found.");
            }

            match.Title = matchDto.Title;
            match.Competition = matchDto.Competition;
            match.MatchDate = matchDto.MatchDate;
            match.Status = matchDto.Status;

            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Match with id {id} not found.");
            }

            _context.Matches.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // for delete
        private async Task<Match> GetByIdAsync(int id)
        {
            return await _context.Matches
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
