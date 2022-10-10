using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ИщуIT.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Vacancy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ItCompanies",
                columns: new[] { "Id", "Address", "Id_Vacancy", "Name", "Phone" },
                values: new object[] { new Guid("3b2c8329-694a-4c53-8bdf-3c42b154d000"), "Saransk", new Guid("94e467fc-e5ae-4005-9f0e-3a05ef051000"), "SimdirSoft", "88005553535" });

            migrationBuilder.InsertData(
                table: "Vacancies",
                columns: new[] { "Id", "Name", "Quantity", "Salary" },
                values: new object[] { new Guid("94e467fc-e5ae-4005-9f0e-3a05ef051000"), "Программист", 2, 100000000 });

            migrationBuilder.InsertData(
                table: "Vacancies",
                columns: new[] { "Id", "Name", "Quantity", "Salary" },
                values: new object[] { new Guid("94e467fc-e5ae-4005-9f0e-3a05ef051001"), "Тестировщик", 20, 20000 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItCompanies");

            migrationBuilder.DropTable(
                name: "Vacancies");
        }
    }
}
