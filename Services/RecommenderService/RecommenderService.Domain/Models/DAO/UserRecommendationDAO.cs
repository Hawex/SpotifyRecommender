using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DAO
{
    public class UserRecommendationDAO : RepositoryEntityBase
    {
        public UserRecommendationDAO(string id, string userId, string trackId, int score)
        {
            Id = id;
            UserId = userId;
            TrackId = trackId;
            Score = score;
        }
        public UserRecommendationDAO(string userId, string trackId, int score)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            TrackId = trackId;
            Score = score;
        }

        public string UserId { get; set; }
        public string TrackId { get; set; }
        public int Score { get; set; }
    }
}
