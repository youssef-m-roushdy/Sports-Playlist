using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.Data;
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
        public async Task<IEnumerable<Match>> GetMatchesByStatus(MatchStatus status)
        {
            return await _context.Matches
                          .Where(m => m.Status == status)
                          .ToListAsync();
        }

        public async Task<Match> GetMatchWithDetails(int matchId)
        {
            return await _context.Matches
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
                            UserName = p.User.UserName
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}