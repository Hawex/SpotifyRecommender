using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Repositories;
using SpotifyAPI;
using SpotifyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.GenerateRecommendationsForUser
{
    public class GenerateRecommendationsForUserCommandHandler : IRequestHandler<GenerateRecommendationsForUserCommand, bool>
    {
        private readonly ILogger<GenerateRecommendationsForUserCommandHandler> _logger;
        private readonly IRecommenderServiceRepository _recommenderServiceRepository;
        private readonly SpotifyRESTApi _spotifyRESTApi;
        private record ChanceBasedTrack(Track track, int chance);
        public GenerateRecommendationsForUserCommandHandler(ILogger<GenerateRecommendationsForUserCommandHandler> logger, IRecommenderServiceRepository recommenderServiceRepository, SpotifyRESTApi spotifyRESTApi)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommenderServiceRepository = recommenderServiceRepository ?? throw new ArgumentNullException(nameof(recommenderServiceRepository));
            _spotifyRESTApi = spotifyRESTApi ?? throw new ArgumentNullException(nameof(spotifyRESTApi));
        }

        public async Task<bool> Handle(GenerateRecommendationsForUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userFavourites = (await _recommenderServiceRepository.GetUserFavourites(request.UserId)) ?? new List<Domain.Models.DAO.UserFavouriteDAO>();
                var queryBuilder = new List<string>();
                if (userFavourites.Any())
                {
                    var favouriteArtists = userFavourites.Where(x => x.EntityType == Domain.Models.Enums.FavouriteEntityType.ARTIST).OrderByDescending(x => x.Score);
                    if (favouriteArtists.Any())
                    {
                        var favouriteArtistWithBestScore = favouriteArtists.Where(x => x.Score == favouriteArtists.First().Score).OrderByDescending(x => x.UpdateTime).First();
                        queryBuilder.Add($"seed_artists={favouriteArtistWithBestScore.EnttityIdentifier}");
                    }
                    var favouriteTracks = userFavourites.Where(x => x.EntityType == Domain.Models.Enums.FavouriteEntityType.TRACK).OrderByDescending(x => x.Score);
                    if (favouriteTracks.Any())
                    {
                        var favouriteTracksWithBestScore = favouriteTracks.OrderByDescending(x => x.UpdateTime).Take(3);
                        queryBuilder.Add($"seed_tracks={string.Join("%2C", favouriteTracksWithBestScore.Select(x => x.EnttityIdentifier))}");
                    }
                    var favouriteGenres = userFavourites.Where(x => x.EntityType == Domain.Models.Enums.FavouriteEntityType.GENRE).OrderByDescending(x => x.Score);
                    if (favouriteGenres.Any())
                    {
                        var favouriteGenresWithBestScore = favouriteGenres.Where(x => x.Score == favouriteGenres.First().Score).OrderByDescending(x => x.UpdateTime).First();
                        queryBuilder.Add($"seed_genres={favouriteGenresWithBestScore.EnttityIdentifier}");
                    }
                    if (queryBuilder.Any())
                    {
                        var query = string.Join("&", queryBuilder);
                        var spotifyRecommendations = await _spotifyRESTApi.GetRecommendationsForQuery(query, 100);
                        var ratingForArtist = GenerateRatingForFavourites(favouriteArtists.ToDictionary(x => x.EnttityIdentifier, x => x.Score));
                        var ratingForTracks = GenerateRatingForFavourites(favouriteTracks.ToDictionary(x => x.EnttityIdentifier, x => x.Score));
                        var ratingForGenres = GenerateRatingForFavourites(favouriteGenres.ToDictionary(x => x.EnttityIdentifier, x => x.Score));
                        List<ChanceBasedTrack> listOfTracksWitchChangeToBeSelected = new();
                        foreach (var recommendation in spotifyRecommendations)
                        {
                            int chances = recommendation.popularity;
                            if (chances < 1)
                                chances = 1;
                            var artistsConnectedWithThisTrack = recommendation?.artists;
                            chances += ratingForArtist?.Where(x => artistsConnectedWithThisTrack?.Select(x => x.id)?.Contains(x.Key) == true)?.Sum(x => x.Value) ?? 0;
                            if (ratingForTracks.ContainsKey(recommendation.id))
                                chances += ratingForTracks[recommendation.id];
                            var genresConnectedWithThisTrack = artistsConnectedWithThisTrack.Where(x=>x.genres != null)?.SelectMany(x => x.genres);
                            chances += ratingForGenres?.Where(x => genresConnectedWithThisTrack?.Contains(x.Key) == true)?.Sum(x => x.Value) ?? 0;

                            listOfTracksWitchChangeToBeSelected.Add(new ChanceBasedTrack(recommendation, chances));
                        }
                        var filteredRecommendations = await BuildRecommendations(request.UserId, listOfTracksWitchChangeToBeSelected);
                        return await _recommenderServiceRepository.UpdateUserRecommendations(filteredRecommendations);
                    }
                }


                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while generating recommendations for user id {userId}: {exception}", request.UserId, ex.ToString());
                throw new Exception($"Exception thrown while executing {System.Reflection.MethodBase.GetCurrentMethod().Name}.", ex);
            }
        }
        private async Task<List<Domain.Models.DAO.UserRecommendationDAO>> BuildRecommendations(string userId, List<ChanceBasedTrack> items)
        {
            var lastListenedTracks = await _recommenderServiceRepository.GetUserRecentlyListenedSongs(userId, 10, 1);
            List<Domain.Models.DAO.UserRecommendationDAO> recommendations = new List<Domain.Models.DAO.UserRecommendationDAO>();

            if (items.Count - items.Count(x => lastListenedTracks.Any(y => y.TrackId.Equals(x.track.id))) < 20)
            {
                while (items.Count > 0)
                {
                    var selectedSong = SelectItem(items);
                    recommendations.Add(new Domain.Models.DAO.UserRecommendationDAO(userId, selectedSong.track.id, selectedSong.chance));
                }
            }
            else
            {
                items.RemoveAll(x => lastListenedTracks.Any(y => y.TrackId.Equals(x.track.id)));
                while (recommendations.Count < 10)
                {
                    var selectedSong = SelectItem(items);
                    recommendations.Add(new Domain.Models.DAO.UserRecommendationDAO(userId, selectedSong.track.id, selectedSong.chance));
                }
            }
            return recommendations;
        }


        // Static method for using from anywhere. You can make its overload for accepting not only List, but arrays also: 
        // public static Item SelectItem (Item[] items)...
        private ChanceBasedTrack SelectItem(List<ChanceBasedTrack> items)
        {
            // Calculate the summa of all portions.
            int poolSize = 0;
            for (int i = 0; i < items.Count; i++)
            {
                poolSize += items[i].chance;
            }
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            // Get a random integer from 0 to PoolSize.
            int randomNumber = rnd.Next(0, poolSize) + 1;

            // Detect the item, which corresponds to current random number.
            int accumulatedProbability = 0;
            for (int i = 0; i < items.Count; i++)
            {
                accumulatedProbability += items[i].chance;
                if (randomNumber <= accumulatedProbability)
                    return items[i];
            }
            return null;
        }


        private Dictionary<string, int> GenerateRatingForFavourites(Dictionary<string, int> userFavouriteScores)
        {
            if (userFavouriteScores.Any())
            {
                Dictionary<string, int> ratedFavourites = new Dictionary<string, int>();
                var maxScore = userFavouriteScores.Max(x => x.Value);
                if (maxScore < 1)
                    maxScore = 1;
                double scale = 100.0d / maxScore;

                foreach (var favourite in userFavouriteScores)
                {
                    var rating = favourite.Value * scale;
                    if (rating < 50)
                        rating = 50;
                    ratedFavourites.Add(favourite.Key, (int)rating);
                }
                return ratedFavourites;

            }
            return new Dictionary<string, int>();

        }
    }
}
