using IdentityModel.Client;
using SpotifyAPI.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI.Auth
{
    public class AccessTokenProvider
    {
        private readonly IdentityClient _identityClient;
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private DateTime _tokenValidToTime;
        public AccessTokenProvider(IdentityClient identityClient)
        {

            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
            _httpClient = new HttpClient();
        }
        public async Task<string> GetAccessToken()
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
                await RequestNewAccessToken();
            if (CheckIfTokenExpired())
            {
                await RequestNewAccessToken();
            }
            return _accessToken;
        }

        private async Task RequestNewAccessToken()
        {
            var tokenResponse = await _httpClient.RequestTokenAsync(new TokenRequest()
            {
                Address = _identityClient.AuthServiceAddress,
                ClientId = _identityClient.ClientId,
                ClientSecret = _identityClient.ClientSecret,
                GrantType = _identityClient.GrantType,
                Parameters = new Parameters(new Dictionary<string, string>() { { "Scope", string.Join(" ", _identityClient.Scopes) } })
            });
            if (tokenResponse.IsError)
                throw new UnauthorizedAccessException($"{tokenResponse.Error}/{tokenResponse.ErrorDescription}");
            if (tokenResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                _accessToken = tokenResponse.AccessToken;
                _tokenValidToTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
            }
        }

        private bool CheckIfTokenExpired()
        {
            return _tokenValidToTime > DateTime.Now.AddSeconds(10);
        }

    }
}
