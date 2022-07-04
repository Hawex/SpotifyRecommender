using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using RecommenderService.API.CQS.AddUser;
using RecommenderService.API.CQS.AddUserFavourites;
using RecommenderService.API.CQS.GenerateRecommendationsForUser;
using RecommenderService.API.CQS.GetUserRecommendations;
using RecommenderService.API.CQS.GetUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.gRPC
{
    public class SpotifyRecommenderService : gRPCSpotifyRecommender.gRPCSpotifyRecommenderBase
    {
        private readonly ILogger<SpotifyRecommenderService> _logger;
        private readonly IMediator _mediator;

        public SpotifyRecommenderService(ILogger<SpotifyRecommenderService> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public override async Task<AddUserReply> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var res = await _mediator.Send(new AddUserCommand(new Domain.Models.DTO.UserDTO(request.Name)));
            return new AddUserReply()
            {
                Result = res
            };
        }
        public override async Task<GetUserRecommendationsReply> GetUserRecommendations(GetUserRecommendationsRequest request, ServerCallContext context)
        {
            var res = await _mediator.Send(new GetUserRecommendationsQuery(request.UserId));
            var reply = new GetUserRecommendationsReply();
            if (res?.Any() == true)
                reply.Result.AddRange(res.Select(x => new UserRecommendation() { Id = x.Id, Score = x.Score, TrackID = x.TrackId, UserId = x.UserId }));
            return reply;
        }

        public override async Task<GetUsersReply> GetUsers(EmptyRequest request, ServerCallContext context)
        {
            var res = await _mediator.Send(new GetUsersQuery());
            var reply = new GetUsersReply();
            if (res?.Any() == true)
                reply.Result.AddRange(res.Select(x => new User() { Id = x.Id, Name = x.Name, IsUserReadyForRecommendation = x.IsUserReadyForRecommendation }));
            return reply;
        }
        public override async Task<AddUserFavouritesReply> AddUserFavourites(AddUserFavouritesRequest request, ServerCallContext context)
        {
            var res = await _mediator.Send(new AddUserFavouritesCommand(request.Favourites.Select(x => new Domain.Models.DTO.UserFavouriteDTO(x.UserId, (Domain.Models.Enums.FavouriteEntityType)Enum.Parse(typeof(Domain.Models.Enums.FavouriteEntityType), x.EntityType.ToString(), true), x.EntityIdentifier, x.Score)).ToList()));
            if (res)
            {
                foreach (var item in request.Favourites.GroupBy(x => x.UserId))
                    await _mediator.Send(new GenerateRecommendationsForUserCommand(item.Key));
            }
            return new AddUserFavouritesReply()
            {
                Result = res
            };
        }

    }
}
