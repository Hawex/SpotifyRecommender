using Microsoft.EntityFrameworkCore.Migrations;

namespace RecommenderService.Infrastructure.Migrations
{
    public partial class creatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserRecommendations",
                newName: "UserRecommendations",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserRecentlyListenedSongs",
                newName: "UserRecentlyListenedSongs",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserFavourites",
                newName: "UserFavourites",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "dbo",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRecommendations",
                schema: "dbo",
                newName: "UserRecommendations");

            migrationBuilder.RenameTable(
                name: "UserRecentlyListenedSongs",
                schema: "dbo",
                newName: "UserRecentlyListenedSongs");

            migrationBuilder.RenameTable(
                name: "UserFavourites",
                schema: "dbo",
                newName: "UserFavourites");
        }
    }
}
