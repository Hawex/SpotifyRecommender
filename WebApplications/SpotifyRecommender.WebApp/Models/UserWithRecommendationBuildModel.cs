using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.WebApp.Models
{
    public class UserWithRecommendationBuildModel
    {
        public string userid{ get; set; }
        public IEnumerable<string> genres { get; set; }
        public IEnumerable<string> artistsids { get; set; }
        public IEnumerable<string> trackids { get; set; }

    }
}
