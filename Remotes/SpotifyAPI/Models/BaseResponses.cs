using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI.Models
{
    public class BaseResponses
    {

        public class AlbumsBase
        {
            public string href { get; set; }
            public List<Album> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public string previous { get; set; }
            public int total { get; set; }
        }

        public class ArtistsBase
        {
            public string href { get; set; }
            public List<Artist> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public string previous { get; set; }
            public int total { get; set; }
        }

        public class EpisodesBase
        {
            public string href { get; set; }
            public List<object> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public string previous { get; set; }
            public int total { get; set; }
        }

        public class PlaylistsBase
        {
            public string href { get; set; }
            public List<object> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public string previous { get; set; }
            public int total { get; set; }
        }

       

        public class ShowsBase
        {
            public string href { get; set; }
            public List<object> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public string previous { get; set; }
            public int total { get; set; }
        }

        public class TracksBase
        {
            public string href { get; set; }
            public List<Track> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public string previous { get; set; }
            public int total { get; set; }
        }
    }
}
