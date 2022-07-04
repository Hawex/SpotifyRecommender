using SpotifyRecommender.BFF.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommender.BFF.Models
{
    public class UserFavourite
    {      
        public string Id { get; init; }
        public string UserId { get; set; }
        public string EntityIdentifier { get; set; }
        public FavouriteEntityType EntityType { get; set; }
        public int Score { get; set; }

    }
}
