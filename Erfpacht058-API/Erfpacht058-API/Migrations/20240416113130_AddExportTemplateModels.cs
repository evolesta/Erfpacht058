using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class AddExportTemplateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExportId",
                table: "Gebruiker",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Export",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Formaat = table.Column<int>(type: "int", nullable: false),
                    AanmaakDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Export", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoortTaak = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Prioriteit = table.Column<int>(type: "int", nullable: false),
                    AanmaakDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AfgerondDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskQueue_Export_ExportId",
                        column: x => x.ExportId,
                        principalTable: "Export",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExportId = table.Column<int>(type: "int", nullable: true),
                    AanmaakDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigingsDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_Export_ExportId",
                        column: x => x.ExportId,
                        principalTable: "Export",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RapportData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RapportData_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExportId", "Wachtwoord" },
                values: new object[] { null, "$2a$11$SFtGvsdL7IZ6Wm..I3DSreEJlCfMztpOyVkehh7cdd.RLjbwEibX2" });

            migrationBuilder.CreateIndex(
                name: "IX_Gebruiker_ExportId",
                table: "Gebruiker",
                column: "ExportId",
                unique: true,
                filter: "[ExportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RapportData_TemplateId",
                table: "RapportData",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskQueue_ExportId",
                table: "TaskQueue",
                column: "ExportId",
                unique: true,
                filter: "[ExportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Template_ExportId",
                table: "Template",
                column: "ExportId",
                unique: true,
                filter: "[ExportId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Gebruiker_Export_ExportId",
                table: "Gebruiker",
                column: "ExportId",
                principalTable: "Export",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gebruiker_Export_ExportId",
                table: "Gebruiker");

            migrationBuilder.DropTable(
                name: "RapportData");

            migrationBuilder.DropTable(
                name: "TaskQueue");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "Export");

            migrationBuilder.DropIndex(
                name: "IX_Gebruiker_ExportId",
                table: "Gebruiker");

            migrationBuilder.DropColumn(
                name: "ExportId",
                table: "Gebruiker");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$Z8YcmKs6xz3hPbHRTNDVUOq60htPap6TAi1syz8FWGf1sUr4CIREK");
        }
    }
}
