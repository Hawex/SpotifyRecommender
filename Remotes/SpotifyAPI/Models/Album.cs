using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpotifyAPI.Models.BaseResponses;

namespace SpotifyAPI.Models
{
    public class Album
    {
        public string album_type { get; set; }
        public int total_tracks { get; set; }
        public List<string> available_markets { get; set; }
        public object external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<object> images { get; set; }
        public string name { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public object restrictions { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public List<Artist> artists { get; set; }
        public TracksBase tracks { get; set; }
     

    }
}
