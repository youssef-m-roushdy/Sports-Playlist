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
        private readonly UserManager<User> _userManager;

        public PlaylistRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddPlaylistToUser(string userId, int matchId)
        {
            var currentLoggedInUser = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (currentLoggedInUser == null)
            {
                throw new KeyNotFoundException("Can't add match to undefined user.");
            }

            var match = await _context.Matches.FindAsync(matchId);
            if (match == null)
            {
                throw new KeyNotFoundException("Can't add a removed or non-existent match to playlist.");
            }

            var newPlaylist = new Playlist
            {
                UserId = userId,
                MatchId = matchId
            };

            await _context.Playlists.AddAsync(newPlaylist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMatchFromPlayList(int id)
        {
            var playlistItem = await _context.Playlists.FindAsync(id);
            if (playlistItem == null)
            {
                throw new KeyNotFoundException("Playlist item not found.");
            }

            _context.Playlists.Remove(playlistItem);
            await _context.SaveChangesAsync();
        }

        
    }
}
