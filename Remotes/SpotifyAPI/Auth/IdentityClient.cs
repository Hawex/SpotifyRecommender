using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI.Auth
{
    public class IdentityClient
    {
        public string ClientId => "c3de7f66445a4cbd82b9f7eb680b8ada";
        public string ClientSecret => "ac85c24f2c314624b4d70b0e844ecb8d";
        public string GrantType => "client_credentials";
        public string AuthServiceAddress => "https://accounts.spotify.com/api/token";
        public string[] Scopes => new string[] { };
    }
}
