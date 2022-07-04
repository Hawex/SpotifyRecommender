using RecommenderService.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DAO
{
    public class UserFavouriteDAO : RepositoryEntityBase
    {
        public UserFavouriteDAO(string id, string userId, FavouriteEntityType entityType, string enttityIdentifier, int score, DateTime updateTime)
        {
            Id = id;
            UserId = userId;
            EntityType = entityType;
            EnttityIdentifier = enttityIdentifier;
            Score = score;
        }
        public UserFavouriteDAO(string userId, FavouriteEntityType entityType, string enttityIdentifier, int score)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            EntityType = entityType;
            EnttityIdentifier = enttityIdentifier;
            Score = score;

        }

        public string UserId { get; set; }
        public string EnttityIdentifier { get; set; }
        public FavouriteEntityType EntityType { get; set; }
        public int Score { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
