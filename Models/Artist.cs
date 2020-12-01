using System;
using System.Collections.Generic;
using System.Text;

namespace KisaRisaMusicCore.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}
