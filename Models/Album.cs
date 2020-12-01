using System;
using System.Collections.Generic;
using System.Text;

namespace KisaRisaMusicCore.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public int ArtistId { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
