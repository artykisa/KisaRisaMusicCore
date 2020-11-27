using System.Collections.Generic;
using KisaMusic.Domain.Models;
using KisaRisaMusicCore.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore.Internal;

namespace KisaRisaMusicCore
{
    public class SampleData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.FileKisas.Any())
            {
                context.FileKisas.AddRange(
                    new FileKisa()
                    {
                        Id = 1,
                        Name = "Apple",
                        Path = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"
                    }
                );
                context.SaveChanges();
            }
        
            if (!context.Tracks.Any())
            {
                context.Tracks.AddRange(
                    new Track()
                    {
                        Id = 1,
                        Title = "Apple",
                        FileKisa = context.FileKisas.Find(1),
                        AlbumId = 1,
                        ArtistId = 1
                        
                    }
                );
                context.SaveChanges();
            }
            if (!context.Playlists.Any())
            {
                List<Track> TrackList = new List<Track>();
                TrackList.Add(context.Tracks.Find(1));
                context.Playlists.AddRange(
                    new Playlist()
                    {
                        Id = 1,
                        Title = "Apple",
                        //Tracks = TrackList
                    }
                );
                context.SaveChanges();
            }
            if (!context.GlobalPlaylists.Any())
            {
                context.GlobalPlaylists.AddRange(
                    
                    new GlobalPlaylist()
                    {
                        Id = 1,
                        Playlists = context.Playlists.Find(1)
                        
                    }
                );
                context.SaveChanges();
            }

            if (!context.Artists.Any())
            {
                context.Artists.AddRange(
                    
                    new Artist()
                    {
                        Id = 1,
                        Nickname = "Kendrick Lamar"
                        
                    }
                );
                context.SaveChanges();
            }
            
            if (!context.Albums.Any())
            {
                context.Albums.AddRange(
                    
                    new Album()
                    {
                        Id = 1,
                        Title = "Damn.",
                        ArtistId = 1
                        
                    }
                );
                context.SaveChanges();
            }
        }
    }
}