using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_JwtAuthTest.Migrations
{
    public partial class ModelEidt_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRefreshToken",
                columns: table => new
                {
                    idUserRefreshToken = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idUser = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RevokeTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IpCreated = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshToken", x => x.idUserRefreshToken);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRefreshToken");
        }
    }
}
