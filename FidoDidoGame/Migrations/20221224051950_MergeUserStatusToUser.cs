using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FidoDidoGame.Migrations
{
    public partial class MergeUserStatusToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_status");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "user",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "user");

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "char(9)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.StatusCode);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_status",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<string>(type: "char(9)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_status", x => new { x.UserId, x.StatusCode });
                    table.ForeignKey(
                        name: "FK_user_status_status_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "status",
                        principalColumn: "StatusCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_status_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_user_status_StatusCode",
                table: "user_status",
                column: "StatusCode");
        }
    }
}
