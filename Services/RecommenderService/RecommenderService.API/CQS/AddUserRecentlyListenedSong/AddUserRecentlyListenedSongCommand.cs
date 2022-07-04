using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.API.CQS.AddUserRecentlyListenedSong
{
    public class AddUserRecentlyListenedSongCommand : IRequest<bool>
    {
        public AddUserRecentlyListenedSongCommand(string userId, string trackId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException($"'{nameof(userId)}' cannot be null or whitespace.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(trackId))
            {
                throw new ArgumentException($"'{nameof(trackId)}' cannot be null or whitespace.", nameof(trackId));
            }

            UserId = userId;
            TrackId = trackId;
        }

        public string UserId { get; }
        public string TrackId { get; }
    }
}
