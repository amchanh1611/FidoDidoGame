using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FidoDidoGame.Migrations
{
    public partial class EditModelFidoDido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "IdCard",
                table: "user",
                type: "char(15)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SpecialStatus",
                table: "point_detail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "fido_dido",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(9)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<sbyte>(
                name: "SpecialStatus",
                table: "fido_dido",
                type: "tinyint",
                nullable: false,
                defaultValue: (sbyte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCard",
                table: "user");

            migrationBuilder.DropColumn(
                name: "SpecialStatus",
                table: "point_detail");

            migrationBuilder.DropColumn(
                name: "SpecialStatus",
                table: "fido_dido");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "user",
                type: "text",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Point",
                table: "fido_dido",
                type: "char(9)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
