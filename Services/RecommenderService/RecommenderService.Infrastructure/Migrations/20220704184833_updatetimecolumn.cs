using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecommenderService.Infrastructure.Migrations
{
    public partial class updatetimecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                schema: "dbo",
                table: "UserFavourites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateTime",
                schema: "dbo",
                table: "UserFavourites");
        }
    }
}
