using Newtonsoft.Json;
using SpotifyAPI.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI
{
    internal class ApiCaller
    {
        private readonly Uri _apiAddress;
        private readonly AccessTokenProvider _accessTokenProvider;

        internal ApiCaller(string apiAddress, AccessTokenProvider accessTokenProvider)
        {
            if (string.IsNullOrWhiteSpace(apiAddress))
            {
                throw new ArgumentException($"'{nameof(apiAddress)}' cannot be null or whitespace.", nameof(apiAddress));
            }
            if (!Uri.TryCreate(apiAddress, UriKind.Absolute, out _apiAddress))
                throw new ArgumentException($"'{nameof(apiAddress)}' should be a valid uri!", nameof(apiAddress));
            _accessTokenProvider = accessTokenProvider;
        }


        internal async Task<HttpResponseMessage> Get(string path)
        {
            using (var client = await GetHttpClient())
            {
                var res = await client.GetAsync(path);
                await CheckIfSuccessStatusCode(res);
                return res;
            }
        }

        internal async Task<HttpResponseMessage> Post(string path, HttpContent httpContent)
        {
            using (var client = await GetHttpClient())
            {
                var res = await client.PostAsync(path, httpContent);
                await CheckIfSuccessStatusCode(res);
                return res;
            }
        }

        internal async Task<T> GetStringResponseAs<T>(HttpResponseMessage httpResponseMessage)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            if (typeof(T).Equals(typeof(string)))
                return (T)Convert.ChangeType(responseString, typeof(T));
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        private async Task<HttpClient> GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = _apiAddress;
            if (_accessTokenProvider != null)
            {
                var header = new AuthenticationHeaderValue("Bearer", await _accessTokenProvider.GetAccessToken());
                httpClient.DefaultRequestHeaders.Authorization = header;
            }
            return httpClient;
        }

        private async Task CheckIfSuccessStatusCode(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                string resultMessage = string.Empty;
                try
                {
                    resultMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                catch { }
                throw new HttpRequestException($"Request failed: {httpResponseMessage.StatusCode}/{httpResponseMessage.ReasonPhrase}/{resultMessage}");
            }

        }

    }
}
