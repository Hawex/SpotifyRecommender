using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Models.DAO
{
    public class UserRecentlyListenedSongDAO : RepositoryEntityBase
    {
        public UserRecentlyListenedSongDAO(string id, string trackId, string userId, DateTime lastPlayedTime)
        {
            Id = id;
            TrackId = trackId;
            UserId = userId;
            LastPlayedTime = lastPlayedTime;
        }
        public UserRecentlyListenedSongDAO(string trackId, string userId, DateTime lastPlayedTime)
        {
            Id = Guid.NewGuid().ToString();
            TrackId = trackId;
            UserId = userId;
            LastPlayedTime = lastPlayedTime;
        }

        public string TrackId { get; set; }
        public string UserId { get; set; }
        public DateTime LastPlayedTime { get; set; }

    }
}
