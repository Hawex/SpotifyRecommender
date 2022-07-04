using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI.Models
{
    public class Artist
    {
        public object external_urls { get; set; }
        public object followers { get; set; }
        public List<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<object> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

    }
}
