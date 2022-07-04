using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI.Models
{
    public class Recommendations
    {
        public List<object> seeds { get; set; }
        public List<Track> tracks { get; set; }

    }
}
