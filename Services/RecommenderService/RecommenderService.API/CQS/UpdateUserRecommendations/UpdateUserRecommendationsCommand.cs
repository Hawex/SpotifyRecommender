using MediatR;
using RecommenderService.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.UpdateUserRecommendations
{
    public class UpdateUserRecommendationsCommand : IRequest<bool>
    {
        public UpdateUserRecommendationsCommand(IEnumerable<UserRecommendationDTO> userRecommendations)
        {
            UserRecommendations = userRecommendations ?? throw new ArgumentNullException(nameof(userRecommendations));
        }

        public IEnumerable<UserRecommendationDTO> UserRecommendations { get; }
    }
}
