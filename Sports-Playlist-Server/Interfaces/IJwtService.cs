using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user);
    }
}