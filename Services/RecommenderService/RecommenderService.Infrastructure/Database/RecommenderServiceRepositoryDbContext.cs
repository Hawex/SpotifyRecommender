using Microsoft.EntityFrameworkCore;
using RecommenderService.Domain.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Infrastructure.Database
{
    public class RecommenderServiceRepositoryDbContext : DbContext
    {
        public RecommenderServiceRepositoryDbContext(DbContextOptions<RecommenderServiceRepositoryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDAO>().ToTable("Users", "dbo");
            modelBuilder.Entity<UserFavouriteDAO>().ToTable("UserFavourites", "dbo");
            modelBuilder.Entity<UserRecommendationDAO>().ToTable("UserRecommendations", "dbo");
            modelBuilder.Entity<UserRecentlyListenedSongDAO>().ToTable("UserRecentlyListenedSongs", "dbo");
        }
        public DbSet<UserDAO> Users { get; set; }
        public DbSet<UserFavouriteDAO> UserFavourites { get; set; }
        public DbSet<UserRecommendationDAO> UserRecommendations { get; set; }
        public DbSet<UserRecentlyListenedSongDAO> UserRecentlyListenedSongs { get; set; }
    }
}
