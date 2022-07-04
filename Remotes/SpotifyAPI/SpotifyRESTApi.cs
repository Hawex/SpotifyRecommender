using Microsoft.Extensions.Configuration;
using SpotifyAPI.Auth;
using SpotifyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAPI
{
    public class SpotifyRESTApi
    {
        private ApiCaller _apiCaller;
        public SpotifyRESTApi(IConfiguration configuration, AccessTokenProvider accessTokenProvider)
        {
            var configurationSection = configuration.GetSection("SpotifyRESTApiConfiguration");

            var aPIAddress = configurationSection["APIAddress"] ?? throw new ArgumentNullException("APIAddress not provided in SpotifyRESTApiConfiguration configuration section");
            _apiCaller = new ApiCaller(aPIAddress, accessTokenProvider);
        }

        public async Task<List<Artist>> GetArtists(string name)
        {
            return (await _apiCaller.GetStringResponseAs<SearchResponse>(await _apiCaller.Get($"/v1/search?q=artist:{name}&type=artist")))?.artists?.items ?? new List<Artist>();
        }

        public async Task<List<Track>> GetTracks(string name)
        {
            return (await _apiCaller.GetStringResponseAs<SearchResponse>(await _apiCaller.Get($"/v1/search?q=track:{name}&type=track")))?.tracks?.items ?? new List<Track>();
        }
        public async Task<Track> GetTrack(string id)
        {
            return await _apiCaller.GetStringResponseAs<Track>(await _apiCaller.Get($"/v1/tracks/{id}"));
        }
        public async Task<List<Track>> GetRecommendationsForQuery(string query, int limit)
        {
            return (await _apiCaller.GetStringResponseAs<Recommendations>(await _apiCaller.Get($"/v1/recommendations?limit={limit}&{query}")))?.tracks ?? new List<Track>();
        }

        public async Task<(List<Track>, List<Album>)> GetTracksAndAlbumsByYear(DateTime yearFrom, DateTime? yearTo = null)
        {
            var yearQuery = $"year:{yearFrom.Year}";
            if (yearTo.HasValue && yearTo > yearFrom)
                yearQuery += $"-{yearTo.Value.Year}";
            var res = await _apiCaller.GetStringResponseAs<SearchResponse>(await _apiCaller.Get($"/v1/search?q={yearQuery}&type=track,album"));
            if (res != null)
            {
                return (res.tracks?.items ?? new List<Track>(), res.albums?.items ?? new List<Album>());
            }
            return (new List<Track>(), new List<Album>());
        }

        public async Task<List<string>> GetGenres()
        {
            return (await _apiCaller.GetStringResponseAs<GenresResponse>(await _apiCaller.Get($"/v1/recommendations/available-genre-seeds")))?.genres ?? new List<string>();
        }


    }
}
