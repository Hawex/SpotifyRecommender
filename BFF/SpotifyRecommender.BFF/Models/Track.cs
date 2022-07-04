using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.BFF.Models
{
    public class Track
    {
        public string Id { get; set; }
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
    }
}
