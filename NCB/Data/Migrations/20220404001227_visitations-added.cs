using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HMS.Data.Migrations
{
    public partial class visitationsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "Patient",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Visitations",
                columns: table => new
                {
                    VisitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Patient = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitations", x => x.VisitID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visitations");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "Patient");
        }
    }
}
