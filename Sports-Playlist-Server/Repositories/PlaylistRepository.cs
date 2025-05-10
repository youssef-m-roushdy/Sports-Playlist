using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.Data;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Interfaces;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _context;

        public PlaylistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPlaylistToUser(string userId, int matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null)
            {
                throw new KeyNotFoundException("Can't add a removed or non-exist match to playlist.");
            }

            var matchInUserPlaylist = await _context.Playlists.AnyAsync(x => x.MatchId == matchId && x.UserId == userId);
            if (matchInUserPlaylist)
            {
                throw new InvalidOperationException("Can't add an existing match to playlist.");
            }

            var newPlaylist = new Playlist
            {
                UserId = userId,
                MatchId = matchId
            };

            await _context.Playlists.AddAsync(newPlaylist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchFromPlayList(string userId, int matchId)
        {
            var matchExist = await _context.Matches.AnyAsync(x => x.Id == matchId);
            if (!matchExist)
            {
                throw new InvalidOperationException("Can't remove an non-existing match from playlist.");
            }

            var playlistItem = _context.Playlists.FirstOrDefault(x => x.UserId == userId && x.MatchId == matchId);
            _context.Playlists.Remove(playlistItem!);
            await _context.SaveChangesAsync();
        }       
    }
}
