using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KisaRisaMusicCore.Models;
using IdentityDbContext = Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext;

namespace KisaRisaMusicCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<FileKisa> FileKisas { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackUser> TrackUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Album>()
                .HasOne<Artist>(e => e.Artist)
                .WithMany(d => d.Albums)
                .HasForeignKey(e => e.ArtistId);
            builder.Entity<IdentityUserLogin>().HasNoKey();
        }


    }
}