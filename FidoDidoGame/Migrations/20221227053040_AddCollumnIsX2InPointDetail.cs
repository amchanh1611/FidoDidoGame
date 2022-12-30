using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FidoDidoGame.Migrations
{
    public partial class AddCollumnIsX2InPointDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsX2",
                table: "point_detail",
                type: "char(9)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsX2",
                table: "point_detail");
        }
    }
}
