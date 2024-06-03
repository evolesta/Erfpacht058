using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoicingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactuurJobId",
                table: "TaskQueue",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactuurJobId",
                table: "Gebruiker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactureringsPeriode",
                table: "Financien",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FactuurJob",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AanmaakDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AfrondDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FactureringsPeriode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactuurJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factuur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancienId = table.Column<int>(type: "int", nullable: true),
                    EigenaarId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bedrag = table.Column<float>(type: "real", nullable: false),
                    FactuurJobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factuur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factuur_Eigenaar_EigenaarId",
                        column: x => x.EigenaarId,
                        principalTable: "Eigenaar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factuur_FactuurJob_FactuurJobId",
                        column: x => x.FactuurJobId,
                        principalTable: "FactuurJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factuur_Financien_FinancienId",
                        column: x => x.FinancienId,
                        principalTable: "Financien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FactuurRegels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aantal = table.Column<int>(type: "int", nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prijs = table.Column<float>(type: "real", nullable: false),
                    Totaal = table.Column<float>(type: "real", nullable: false),
                    FactuurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactuurRegels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactuurRegels_Factuur_FactuurId",
                        column: x => x.FactuurId,
                        principalTable: "Factuur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FactuurJobId", "Wachtwoord" },
                values: new object[] { null, "$2a$11$PER98YhJOWbpEIgsN/7xRew5tuZCv00foOUJCEvh1SlRpHK5bCuka" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskQueue_FactuurJobId",
                table: "TaskQueue",
                column: "FactuurJobId",
                unique: true,
                filter: "[FactuurJobId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Gebruiker_FactuurJobId",
                table: "Gebruiker",
                column: "FactuurJobId",
                unique: true,
                filter: "[FactuurJobId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Factuur_EigenaarId",
                table: "Factuur",
                column: "EigenaarId");

            migrationBuilder.CreateIndex(
                name: "IX_Factuur_FactuurJobId",
                table: "Factuur",
                column: "FactuurJobId");

            migrationBuilder.CreateIndex(
                name: "IX_Factuur_FinancienId",
                table: "Factuur",
                column: "FinancienId");

            migrationBuilder.CreateIndex(
                name: "IX_FactuurRegels_FactuurId",
                table: "FactuurRegels",
                column: "FactuurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gebruiker_FactuurJob_FactuurJobId",
                table: "Gebruiker",
                column: "FactuurJobId",
                principalTable: "FactuurJob",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskQueue_FactuurJob_FactuurJobId",
                table: "TaskQueue",
                column: "FactuurJobId",
                principalTable: "FactuurJob",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gebruiker_FactuurJob_FactuurJobId",
                table: "Gebruiker");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskQueue_FactuurJob_FactuurJobId",
                table: "TaskQueue");

            migrationBuilder.DropTable(
                name: "FactuurRegels");

            migrationBuilder.DropTable(
                name: "Factuur");

            migrationBuilder.DropTable(
                name: "FactuurJob");

            migrationBuilder.DropIndex(
                name: "IX_TaskQueue_FactuurJobId",
                table: "TaskQueue");

            migrationBuilder.DropIndex(
                name: "IX_Gebruiker_FactuurJobId",
                table: "Gebruiker");

            migrationBuilder.DropColumn(
                name: "FactuurJobId",
                table: "TaskQueue");

            migrationBuilder.DropColumn(
                name: "FactuurJobId",
                table: "Gebruiker");

            migrationBuilder.DropColumn(
                name: "FactureringsPeriode",
                table: "Financien");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$QcbyWzSwQRFWx5EBlxG2Q.ICvieefhwsv6SQjmltWOWlxoUSKkjry");
        }
    }
}
