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
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        private readonly ApplicationDbContext _context;
        public MatchRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
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
                              MatchUrl = p.MatchUrl,
                              Status = p.Status
                          })
                          .ToListAsync();
        }

        public async Task<Match> GetMatchWithDetails(int matchId)
        {
            var matches = await _context.Matches
                .Where(m => m.Id == matchId)
                .Select(m => new Match
                {
                    Id = m.Id,
                    Title = m.Title,
                    Competition = m.Competition,
                    MatchDate = m.MatchDate,
                    Status = m.Status,
                    Playlists = m.Playlists.Select(p => new Playlist
                    {
                        Id = p.Id,
                        UserId = p.UserId,
                        MatchId = p.MatchId,
                        User = new User
                        {
                            Id = p.User.Id,
                            UserName = p.User.UserName,
                            FirstName = p.User.FirstName,
                            LastName = p.User.LastName
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (matches == null)
            {
                throw new KeyNotFoundException($"Match with id {matchId} not found.");
            }

            return matches;
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
                    MatchUrl = p.Match.MatchUrl,
                    Status = p.Match.Status
                })
                .ToListAsync();
        }
    }
}