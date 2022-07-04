using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecommenderService.Domain.Models.DAO;
using RecommenderService.Domain.Models.Enums;
using RecommenderService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Infrastructure.Database.Repositiories
{
    public class SqlRecommenderServiceRepository : IRecommenderServiceRepository
    {
        private readonly RecommenderServiceRepositoryDbContext _dbContext;

        public SqlRecommenderServiceRepository(RecommenderServiceRepositoryDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> AddUser(UserDAO user)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(x => x.Name == user.Name);
            if (dbUser != null)
                throw new Exception($"User with name {user.Name} already exists!");
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> AddUserFavourites(IEnumerable<UserFavouriteDAO> userFavouriteDAO)
        {
            var grouped = userFavouriteDAO.GroupBy(x => x.UserId);
            foreach (var group in grouped)
            {
                var userFavourites = await GetUserFavourites(group.Key);
                var filtered = group.Where(x => !userFavourites.Any(y => x.EntityType == y.EntityType && x.EnttityIdentifier == y.EnttityIdentifier));
                if (filtered.Any())
                {
                    foreach (var item in filtered)
                    {
                        item.UpdateTime = DateTime.Now;
                    }
                    await _dbContext.AddRangeAsync(filtered);
                }
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserRecentlyListenedSong(UserRecentlyListenedSongDAO recentlyListenedSongDAO)
        {
            var lastListenedSongs = _dbContext.UserRecentlyListenedSongs.Where(x => x.UserId == recentlyListenedSongDAO.UserId).ToList();
            var currentSongInDb = lastListenedSongs.FirstOrDefault(x => x.TrackId == recentlyListenedSongDAO.TrackId);
            if (currentSongInDb != null)
            {
                currentSongInDb.LastPlayedTime = recentlyListenedSongDAO.LastPlayedTime;
                _dbContext.UserRecentlyListenedSongs.Update(currentSongInDb);
            }
            else
            {
                await _dbContext.AddAsync(recentlyListenedSongDAO);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDAO>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<IEnumerable<UserFavouriteDAO>> GetUserFavourites(string userId)
        {
            return await _dbContext.UserFavourites.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserRecommendationDAO>> GetUserRecommendations(string userId)
        {
            return await _dbContext.UserRecommendations.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateUserFavouritesScore(string userFavouriteId, int score)
        {
            var userFavourite = await _dbContext.UserFavourites.FirstOrDefaultAsync(x => x.Id == userFavouriteId);
            if (userFavourite == null)
                return false;
            userFavourite.UpdateTime = DateTime.Now;
            userFavourite.Score = score;
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateUserRecommendations(IEnumerable<UserRecommendationDAO> userRecommendationDAO)
        {
            if (!userRecommendationDAO.Any())
                return true;
            var oldRecommendations = _dbContext.UserRecommendations.Where(x => x.UserId == userRecommendationDAO.First().UserId);
            _dbContext.RemoveRange(oldRecommendations);
            await _dbContext.AddRangeAsync(userRecommendationDAO);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<UserRecentlyListenedSongDAO>> GetUserRecentlyListenedSongs(string userId, int limit, int page)
        {
            return await _dbContext.UserRecentlyListenedSongs.Where(x => x.UserId == userId).OrderByDescending(x=>x.LastPlayedTime).Skip(limit * page).Take(limit).ToListAsync();
        }
    }
}
