using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandTripAPI.Migrations
{
    public partial class newcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1,
                column: "Key",
                value: "none");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Key",
                value: "none");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "Salt" },
                values: new object[] { "eK6nyeTlRBTISJKWEkgCwFYpv307/5jdqo3IbWgjB9s=", new byte[] { 33, 97, 110, 210, 5, 200, 180, 52, 38, 136, 67, 181, 75, 28, 95, 241 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Seasons");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "Salt" },
                values: new object[] { "iype2OKlqlOUlV7f91Sv0qbjYdPSAFEgvuaddd/qK5M=", new byte[] { 192, 144, 241, 115, 96, 29, 142, 106, 193, 121, 107, 157, 114, 34, 15, 26 } });
        }
    }
}
