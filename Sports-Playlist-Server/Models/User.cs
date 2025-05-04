using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sports_Playlist_Server.Models
{
    public class User : IdentityUser
    {
        public ICollection<Playlist> Playlists { get; set; }
    }
}