using System.Collections.Generic;
using KisaRisaMusicCore.Data;
using KisaRisaMusicCore.Models;
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
                    },
                    new FileKisa()
                    {
                        Id = 2,
                        Name = "Tomato",
                        Path = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3"
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
                        Title = "HUMBLE.",
                        FileKisaId = 1,
                        AlbumId = 1,
                        ArtistId = 1
                    },
                    new Track()
                    {
                        Id = 2,
                        Title = "A$AP Forever",
                        FileKisaId = 2,
                        AlbumId = 2,
                        ArtistId = 2
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
                    },
                    new Artist()
                    {
                        Id = 2,
                        Nickname = "A$AP Rocky"
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
                    },
                    new Album()
                    {
                        Id = 2,
                        Title = "Testing",
                        ArtistId = 2
                    }
                );
                context.SaveChanges();
            }

        }
    }
}