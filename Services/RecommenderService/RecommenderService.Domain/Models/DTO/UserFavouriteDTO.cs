using RecommenderService.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DTO
{
    public class UserFavouriteDTO
    {
        public UserFavouriteDTO(string id, string userId, FavouriteEntityType entityType, string enttityIdentifier, int score)
        {
            Id = id;
            UserId = userId;
            EntityType = entityType;
            EntityIdentifier = enttityIdentifier;
            Score = score;
        }
        public UserFavouriteDTO(string userId, FavouriteEntityType entityType, string enttityIdentifier, int score)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            EntityType = entityType;
            EntityIdentifier = enttityIdentifier;
            Score = score;
        }

        public string Id { get; init; }
        public string UserId { get; set; }
        public string EntityIdentifier { get; set; }
        public FavouriteEntityType EntityType { get; set; }
        public int Score { get; set; }

    }
}
