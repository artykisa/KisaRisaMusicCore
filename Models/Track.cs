using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace KisaRisaMusicCore.Models
{
    public class Track
    {
        
        public int Id { get; set; }
        public Album Album{ get; set; }
        public int AlbumId { get; set; }
        public Artist Artist { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; }
        public int FileKisaId { get; set; }
        public FileKisa FileKisa { get; set; }
    }
}
