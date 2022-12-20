using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrandTripAPI.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "Salt", "Username" },
                values: new object[] { 1, "iype2OKlqlOUlV7f91Sv0qbjYdPSAFEgvuaddd/qK5M=", "Admin", new byte[] { 192, 144, 241, 115, 96, 29, 142, 106, 193, 121, 107, 157, 114, 34, 15, 26 }, "teletraan" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
