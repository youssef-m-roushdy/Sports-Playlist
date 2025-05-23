using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.Enums;

namespace Sports_Playlist_Server.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Competition { get; set; }
        public string MatchUrl { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; } // "Live" or "Replay"
        public ICollection<Playlist> Playlists { get; set; }
    }
}