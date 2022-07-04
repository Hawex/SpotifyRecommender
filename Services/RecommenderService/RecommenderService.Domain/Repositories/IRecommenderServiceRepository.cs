using RecommenderService.Domain.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Domain.Repositories
{
    public interface IRecommenderServiceRepository
    {
        Task<bool> AddUser(UserDAO user);
        Task<bool> AddUserFavourites(IEnumerable<UserFavouriteDAO> userFavouriteDAO);
        Task<bool> AddUserRecentlyListenedSong(UserRecentlyListenedSongDAO recentlyListenedSongDAO);

        Task<bool> UpdateUserRecommendations(IEnumerable<UserRecommendationDAO> userRecommendationDAO);
        Task<bool> UpdateUserFavouritesScore(string userFavouriteId, int score);

        Task<IEnumerable<UserDAO>> GetUsers();
        public Task<IEnumerable<UserRecentlyListenedSongDAO>> GetUserRecentlyListenedSongs(string userId, int limit, int page);
        Task<IEnumerable<UserFavouriteDAO>> GetUserFavourites(string userId);
        Task<IEnumerable<UserRecommendationDAO>> GetUserRecommendations(string userId);

    }
}
