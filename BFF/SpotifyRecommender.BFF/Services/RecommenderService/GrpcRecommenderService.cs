using Grpc.Core;
using RecommenderService.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.BFF.Services.RecommenderService
{
    public class GrpcRecommenderService
    {
        private readonly gRPCSpotifyRecommender.gRPCSpotifyRecommenderClient _client;

        public GrpcRecommenderService(gRPCSpotifyRecommender.gRPCSpotifyRecommenderClient client)
        {
            _client = client;
        }

        public async Task<bool> AddUser(string login)
        {
            try
            {
                var res = await _client.AddUserAsync(new AddUserRequest() { Name = login });
                return res.Result;
            }
            catch (RpcException exception)
            {
                return false;
            }
        }

        public async Task<bool> AddUserFavourites(List<SpotifyRecommender.BFF.Models.UserFavourite> userFavourites)
        {
            try
            {
                var request = new AddUserFavouritesRequest();
                request.Favourites.AddRange(userFavourites.Select(x => new UserFavourite() { EntityIdentifier = x.EntityIdentifier, EntityType = (FavouriteEntityType)Enum.Parse(typeof(FavouriteEntityType), x.EntityType.ToString(), true), Score = x.Score, UserId = x.UserId }));
                var res = await _client.AddUserFavouritesAsync(request);
                return res.Result;
            }
            catch (RpcException exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Models.User>> GetUsers()
        {
            try
            {
                var res = await _client.GetUsersAsync(new EmptyRequest());
                return res.Result?.Select(x => new Models.User() { ID = x.Id, Name = x.Name, IsUserReadyForRecommendation = x.IsUserReadyForRecommendation }) ?? new List<Models.User>();
            }
            catch (RpcException exception)
            {
                return new List<Models.User>();
            }
        }
        public async Task<IEnumerable<Models.UserRecommendation>> GetUsersRecommendations(string userId)
        {
            try
            {
                var res = await _client.GetUserRecommendationsAsync(new GetUserRecommendationsRequest() { UserId = userId});
                return res.Result?.Select(x => new Models.UserRecommendation() { Id = x.Id, UserId = x.UserId, Score = x.Score, TrackId = x.TrackID }) ?? new List<Models.UserRecommendation>();
            }
            catch (RpcException exception)
            {
                return new List<Models.UserRecommendation>();
            }
        }
    }

}
