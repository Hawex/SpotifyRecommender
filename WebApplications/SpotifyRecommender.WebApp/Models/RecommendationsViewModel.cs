using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.WebApp.Models
{
    public class RecommendationsViewModel
    {
        public string Id { get; init; }
        public string UserId { get; set; }
        public string TrackId { get; set; }
        public int Score { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
    }
}
