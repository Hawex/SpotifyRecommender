using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI;
using SpotifyRecommender.WebApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyRecommender.WebApp.API
{
    public class SpotifyRecommenderBFF
    {
        private ApiCaller _apiCaller;
        public SpotifyRecommenderBFF(IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("SpotifyRecommenderBFF");

            var aPIAddress = configurationSection["APIAddress"] ?? throw new ArgumentNullException("APIAddress not provided in SpotifyRESTApiConfiguration configuration section");
            _apiCaller = new ApiCaller(aPIAddress);
        }

        public async Task<List<string>> GetGenres()
        {
            return (await _apiCaller.GetStringResponseAs<List<string>>(await _apiCaller.Get($"/Recommender/genres"))) ?? new List<string>();
        }
        public async Task<List<User>> GetUsers()
        {
            return (await _apiCaller.GetStringResponseAs<List<User>>(await _apiCaller.Get($"/Recommender/users"))) ?? new List<User>();
        }

        public async Task<bool> AddUser(string login)
        {
            return await _apiCaller.GetStringResponseAs<bool>(await _apiCaller.Post($"/Recommender/users/add", new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json")));
        }
        public async Task<bool> AddUserFavourites(List<UserFavourite> userFavourites)
        {
            return await _apiCaller.GetStringResponseAs<bool>(await _apiCaller.Post($"/Recommender/users/favourites/add", new StringContent(JsonConvert.SerializeObject(userFavourites), System.Text.Encoding.UTF8, "application/json")));
        }
        public async Task<List<Track>> GetTracks(string name)
        {
            return (await _apiCaller.GetStringResponseAs<List<Track>>(await _apiCaller.Get($"/Recommender/tracks/{name}"))) ?? new List<Track>();
        }
        public async Task<List<Artist>> GetArtists(string name)
        {
            return (await _apiCaller.GetStringResponseAs<List<Artist>>(await _apiCaller.Get($"/Recommender/artists/{name}"))) ?? new List<Artist>();
        }
        public async Task<List<UserRecommendation>> GetRecommendations(string userId)
        {
            return (await _apiCaller.GetStringResponseAs<List<UserRecommendation>>(await _apiCaller.Get($"/Recommender/recommendations/{userId}"))) ?? new List<UserRecommendation>();
        }
        public async Task<Track> GetTrackById(string trackId)
        {
            return (await _apiCaller.GetStringResponseAs<Track>(await _apiCaller.Get($"/Recommender/track/{trackId}")));
        }

    }
}
