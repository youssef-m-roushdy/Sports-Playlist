using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports_Playlist_Server.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}