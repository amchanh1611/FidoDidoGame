using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FidoDidoGame.Migrations
{
    public partial class AddColumnRefreshTokenInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "user");
        }
    }
}
