using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Enums;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Interfaces
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        public Task<IEnumerable<MatchDto>> GetMatchesByStatus(MatchStatus status);
        public Task<MatchDetailsDto> GetMatchWithDetails(int matchId);
        public Task<IEnumerable<MatchDto>> GetUserMatchesFromPlaylist(string userId);
    }
}