using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class AddImportModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImportId",
                table: "TaskQueue",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImportId",
                table: "Gebruiker",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Import",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aanmaakdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigingsDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    importPad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Import", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslateModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportId = table.Column<int>(type: "int", nullable: true),
                    AanmaakDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WijzigingsDatum = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslateModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslateModel_Import_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Import",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CSVColummnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslateModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_TranslateModel_TranslateModelId",
                        column: x => x.TranslateModelId,
                        principalTable: "TranslateModel",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImportId", "Wachtwoord" },
                values: new object[] { null, "$2a$11$QcbyWzSwQRFWx5EBlxG2Q.ICvieefhwsv6SQjmltWOWlxoUSKkjry" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskQueue_ImportId",
                table: "TaskQueue",
                column: "ImportId",
                unique: true,
                filter: "[ImportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Gebruiker_ImportId",
                table: "Gebruiker",
                column: "ImportId",
                unique: true,
                filter: "[ImportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TranslateModel_ImportId",
                table: "TranslateModel",
                column: "ImportId",
                unique: true,
                filter: "[ImportId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Translation_TranslateModelId",
                table: "Translation",
                column: "TranslateModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gebruiker_Import_ImportId",
                table: "Gebruiker",
                column: "ImportId",
                principalTable: "Import",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskQueue_Import_ImportId",
                table: "TaskQueue",
                column: "ImportId",
                principalTable: "Import",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gebruiker_Import_ImportId",
                table: "Gebruiker");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskQueue_Import_ImportId",
                table: "TaskQueue");

            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "TranslateModel");

            migrationBuilder.DropTable(
                name: "Import");

            migrationBuilder.DropIndex(
                name: "IX_TaskQueue_ImportId",
                table: "TaskQueue");

            migrationBuilder.DropIndex(
                name: "IX_Gebruiker_ImportId",
                table: "Gebruiker");

            migrationBuilder.DropColumn(
                name: "ImportId",
                table: "TaskQueue");

            migrationBuilder.DropColumn(
                name: "ImportId",
                table: "Gebruiker");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$QkqRfsIjwS26DMlJkbLTae3pw8Cek3vx.8j6oiF3FZZ1eBy.BHKz6");
        }
    }
}
