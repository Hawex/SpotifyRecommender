using MediatR;
using RecommenderService.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.GetUserRecommendations
{
    public class GetUserRecommendationsQuery : IRequest<IEnumerable<UserRecommendationDTO>>
    {
        public GetUserRecommendationsQuery(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or whitespace.", nameof(userId));
            }

            UserId = userId;
        }

        public string UserId { get; }
    }
}
