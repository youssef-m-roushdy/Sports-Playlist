using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.Enums;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Interfaces
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        public Task<IEnumerable<Match>> GetMatchesByStatus(MatchStatus status);
    }
}