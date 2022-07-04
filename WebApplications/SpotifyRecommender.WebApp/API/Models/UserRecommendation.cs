using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.WebApp.API.Models
{
    public class UserRecommendation
    {
        public string Id { get; init; }
        public string UserId { get; set; }
        public string TrackId { get; set; }
        public int Score { get; set; }
    }
}
