using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Enums;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Interfaces
{
    public interface IMatchRepository
    {
        Task<IEnumerable<MatchDto>> GetAllAsync();
        Task<MatchDetailsDto> GetByIdWithDetails(int id);
        Task AddAsync(Match entity);
        Task UpdateAsync(int id, UpdateMatchDto matchDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<MatchDto>> GetMatchesByStatus(MatchStatus status);
        Task<IEnumerable<MatchDto>> GetMatchesByCompetition(string competition);
        Task<IEnumerable<MatchDto>> GetUserMatchesFromPlaylist(string userId);
    }
}