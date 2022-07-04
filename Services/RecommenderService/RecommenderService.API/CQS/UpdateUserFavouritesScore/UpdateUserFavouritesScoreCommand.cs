using MediatR;
using RecommenderService.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.UpdateUserFavouritesScore
{
    public class UpdateUserFavouritesScoreCommand : IRequest<bool>
    {
        public UpdateUserFavouritesScoreCommand(string userFavouriteId, int score)
        {
            if (string.IsNullOrWhiteSpace(userFavouriteId))
            {
                throw new ArgumentException($"'{nameof(userFavouriteId)}' cannot be null or whitespace.", nameof(userFavouriteId));
            }

            UserFavouriteId = userFavouriteId;
            if (score < 0)
                throw new ArgumentException($"'{nameof(score)}' cannot be less than 0.");
            Score = score;
        }

        public string UserFavouriteId { get; }
        public int Score { get; }
    }
}
