using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_JwtAuthTest.Migrations
{
    public partial class ModelEidt_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActiveIs",
                table: "UserRefreshToken",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExpiredIs",
                table: "UserRefreshToken",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RevokeIs",
                table: "UserRefreshToken",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveIs",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "ExpiredIs",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "RevokeIs",
                table: "UserRefreshToken");
        }
    }
}
