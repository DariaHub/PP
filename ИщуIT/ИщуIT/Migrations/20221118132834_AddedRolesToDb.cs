using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ИщуIT.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d8180ba-21ef-4276-b88c-37df9db851b2", "c694414c-3ef5-4235-8fc7-a685b4618bf2", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2dba017-4d05-4278-9109-4d42584b961d", "415c2342-b4a3-4b0f-9030-473ab4e68a6d", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d8180ba-21ef-4276-b88c-37df9db851b2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2dba017-4d05-4278-9109-4d42584b961d");
        }
    }
}
