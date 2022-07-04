using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.GenerateRecommendationsForUser
{
    public class GenerateRecommendationsForUserCommand : IRequest<bool>
    {
        public GenerateRecommendationsForUserCommand(string userId)
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
