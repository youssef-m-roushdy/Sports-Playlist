using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Interfaces
{
    public interface IPlaylistRepository
    {
        Task AddPlaylistToUser(string userId, int matchId);
        Task DeleteMatchFromPlayList(string userId, int matchId);
    }
}