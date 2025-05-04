using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSets for models
        public DbSet<Match> Matches { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define the many-to-many relationship for Playlist
            builder.Entity<Playlist>()
                .HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Playlist>()
                .HasOne(p => p.Match)
                .WithMany(m => m.Playlists)
                .HasForeignKey(p => p.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}