using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyAPI;
using SpotifyRecommender.BFF.Models;
using SpotifyRecommender.BFF.Services.RecommenderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.BFF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommenderController : ControllerBase
    {

        private readonly ILogger<RecommenderController> _logger;
        private readonly SpotifyRESTApi _spotifyRESTApi;
        private readonly GrpcRecommenderService _grpcRecommenderService;

        public RecommenderController(ILogger<RecommenderController> logger, SpotifyRESTApi spotifyRESTApi, GrpcRecommenderService grpcRecommenderService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _spotifyRESTApi = spotifyRESTApi ?? throw new ArgumentNullException(nameof(spotifyRESTApi));
            _grpcRecommenderService = grpcRecommenderService ?? throw new ArgumentNullException(nameof(grpcRecommenderService));
        }

        [HttpGet]
        [Route("artists/{name}")]
        public async Task<IActionResult> GetArtists(string name)
        {
            return Ok((await _spotifyRESTApi.GetArtists(name))?.OrderByDescending(x => x.popularity).Select(x=> new Artist() {Id = x.id, Name = x.name }));
        }
        
        [HttpPost]
        [Route("users/add")]
        public async Task<IActionResult> AddUser([FromBody]string name)
        {
            return Ok(await _grpcRecommenderService.AddUser(name));
        }
        
        [HttpPost]
        [Route("users/favourites/add")]
        public async Task<IActionResult> AddUserFavourites([FromBody] List<UserFavourite> userFavourites)
        {
            return Ok(await _grpcRecommenderService.AddUserFavourites(userFavourites));
        }
        
        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _grpcRecommenderService.GetUsers());
        }

        [HttpGet]
        [Route("tracks/{name}")]
        public async Task<IActionResult> GetTracks(string name)
        {
            return Ok((await _spotifyRESTApi.GetTracks(name))?.OrderByDescending(x => x.popularity).Select(x => new Track() { Id = x.id, TrackName = x.name, ArtistName = string.Join(" & ", x.artists.Select(x => x.name)) }));
        }

        [HttpGet]
        [Route("genres")]
        public async Task<IActionResult> GetGenres()
        {
            return Ok(await _spotifyRESTApi.GetGenres());
        }

        [HttpGet]
        [Route("genres/{name}")]
        public async Task<IActionResult> GetGenres(string name)
        {
            return Ok((await _spotifyRESTApi.GetGenres())?.Where(x => x.Contains(name, StringComparison.OrdinalIgnoreCase)));
        }
        [HttpGet]
        [Route("recommendations/{userId}")]
        public async Task<IActionResult> GetRecommendations(string userId)
        {
            return Ok(await _grpcRecommenderService.GetUsersRecommendations(userId));
        }
        
        [HttpGet]
        [Route("track/{trackId}")]
        public async Task<IActionResult> GetTrackById(string trackId)
        {
            var track = await _spotifyRESTApi.GetTrack(trackId);
            return Ok(track != null ? new Track() { Id = track.id, TrackName = track.name, ArtistName = string.Join(" & ", track.artists.Select(x => x.name)) } : null);
        }


    }
}
