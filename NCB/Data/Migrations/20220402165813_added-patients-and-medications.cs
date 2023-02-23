using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HMS.Data.Migrations
{
    public partial class addedpatientsandmedications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicine",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Restock = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    DateofBirth = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    TelephoneNumber = table.Column<string>(nullable: false),
                    PatientImage = table.Column<string>(nullable: true),
                    LastVisit = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicine");

            migrationBuilder.DropTable(
                name: "Patient");
        }
    }
}
