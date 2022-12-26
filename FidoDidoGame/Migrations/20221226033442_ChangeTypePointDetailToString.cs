using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FidoDidoGame.Migrations
{
    public partial class ChangeTypePointDetailToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Point",
                table: "point_detail",
                type: "char(9)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "point_detail",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(9)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
