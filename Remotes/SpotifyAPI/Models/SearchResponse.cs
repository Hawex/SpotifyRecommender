using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpotifyAPI.Models.BaseResponses;

namespace SpotifyAPI.Models
{
    public class SearchResponse
    {
        public TracksBase tracks { get; set; }
        public ArtistsBase artists { get; set; }
        public AlbumsBase albums { get; set; }
        public PlaylistsBase playlists { get; set; }
        public ShowsBase shows { get; set; }
        public EpisodesBase episodes { get; set; }
    }
}
