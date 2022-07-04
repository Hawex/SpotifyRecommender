using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DTO
{
    public class UserRecommendationDTO
    {
        public UserRecommendationDTO(string id, string userId, string trackId, int score)
        {
            Id = id;
            UserId = userId;
            TrackId = trackId;
            Score = score;
        }
        public UserRecommendationDTO(string userId, string trackId, int score)
        {
            UserId = userId;
            TrackId = trackId;
            Score = score;
        }
        public string Id { get; init; }
        public string UserId { get; set; }
        public string TrackId { get; set; }
        public int Score { get; set; }
    }
}
