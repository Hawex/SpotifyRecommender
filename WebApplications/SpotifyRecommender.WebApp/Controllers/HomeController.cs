using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyRecommender.WebApp.API;
using SpotifyRecommender.WebApp.API.Models;
using SpotifyRecommender.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpotifyRecommenderBFF _spotifyRecommenderBFF;

        public HomeController(ILogger<HomeController> logger, SpotifyRecommenderBFF spotifyRecommenderBFF)
        {
            _logger = logger;
            _spotifyRecommenderBFF = spotifyRecommenderBFF;
        }

        public IActionResult AddUser()
        {
            return View();
        }

        public async Task<IActionResult> AddNewUser(UserModel userModel)
        {
            ViewBag.AddUserMessage = (await _spotifyRecommenderBFF.AddUser(userModel.Login)) ? "User Added" : "Adding user failed. Please try again.";
            userModel = new UserModel();
            return View("AddUser", userModel);
        }


        public async Task<List<User>> GetUsers()
        {
            return await _spotifyRecommenderBFF.GetUsers();
        }

        public async Task<IActionResult> Recommendations()
        {
            var users = (await _spotifyRecommenderBFF.GetUsers()).Where(x => x.IsUserReadyForRecommendation);
            var usersModel = users.Select(x => new UserModel() { Id = x.ID, Login = x.Name, IsUserReadyForRecommendation = x.IsUserReadyForRecommendation });
            ViewBag.Users = usersModel;
            return View();
        }

        public async Task<List<RecommendationsViewModel>> GetUserRecommendations(string userId)
        {
            var recommendations = await _spotifyRecommenderBFF.GetRecommendations(userId);
            List<RecommendationsViewModel> recommendationsViewModels = new List<RecommendationsViewModel>();
            foreach (var recommendation in recommendations)
            {
                var track = await _spotifyRecommenderBFF.GetTrackById(recommendation.TrackId);
                if (track != null)
                    recommendationsViewModels.Add(new RecommendationsViewModel()
                    {
                        Id = recommendation.Id,
                        TrackId = recommendation.TrackId,
                        Score = recommendation.Score,
                        UserId = recommendation.UserId,
                        TrackName = track.TrackName,
                        ArtistName = track.ArtistName
                    });
            }
            return recommendationsViewModels.OrderByDescending(x => x.Score).ToList();
        }

        public async Task<IActionResult> Index()
        {
            var users = (await _spotifyRecommenderBFF.GetUsers()).Where(x => !x.IsUserReadyForRecommendation);
            var usersModel = users.Select(x => new UserModel() { Id = x.ID, Login = x.Name, IsUserReadyForRecommendation = x.IsUserReadyForRecommendation });
            var genres = await _spotifyRecommenderBFF.GetGenres();
            ViewBag.Genres = genres;
            ViewBag.Users = usersModel;

            return View();
        }

        public async Task<List<Track>> GetTracks(string name)
        {
            return await _spotifyRecommenderBFF.GetTracks(name);
        }

        public async Task<List<Artist>> GetArtists(string name)
        {
            return await _spotifyRecommenderBFF.GetArtists(name);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<bool> SaveUserRecommendations([FromBody] UserWithRecommendationBuildModel model)
        {
            if (model?.userid != null)
            {
                List<UserFavourite> userFavourites = new List<UserFavourite>();
                if (model.artistsids?.Count() > 0)
                    userFavourites.AddRange(model.artistsids.Select(x => new UserFavourite()
                    {
                        EntityIdentifier = x,
                        EntityType = API.Models.Enums.FavouriteEntityType.ARTIST,
                        Score = 1,
                        UserId = model.userid
                    }));
                if (model.genres?.Count() > 0)
                    userFavourites.AddRange(model.genres.Select(x => new UserFavourite()
                    {
                        EntityIdentifier = x,
                        EntityType = API.Models.Enums.FavouriteEntityType.GENRE,
                        Score = 1,
                        UserId = model.userid
                    }));
                if (model.trackids?.Count() > 0)
                    userFavourites.AddRange(model.artistsids.Select(x => new UserFavourite()
                    {
                        EntityIdentifier = x,
                        EntityType = API.Models.Enums.FavouriteEntityType.TRACK,
                        Score = 1,
                        UserId = model.userid
                    }));
                if (userFavourites.Any())
                {
                    return await _spotifyRecommenderBFF.AddUserFavourites(userFavourites);
                }

            }
            return false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
