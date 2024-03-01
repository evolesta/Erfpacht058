using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class AddEigendomData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Wachtwoord",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Voornamen",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Emailadres",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Eigenaar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voornamen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voorletters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Straatnaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Huisnummer = table.Column<int>(type: "int", nullable: false),
                    Toevoeging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Woonplaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debiteurnummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ingangsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Einddatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eigenaar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eigendom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Relatienummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ingangsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Einddatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Complexnummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EconomischeWaarde = table.Column<float>(type: "real", nullable: false),
                    VerzekerdeWaarde = table.Column<float>(type: "real", nullable: false),
                    Notities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eigendom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EigendomId = table.Column<int>(type: "int", nullable: true),
                    Straatnaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Huisnummer = table.Column<int>(type: "int", nullable: false),
                    Toevoeging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Woonplaats = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adres_Eigendom_EigendomId",
                        column: x => x.EigendomId,
                        principalTable: "Eigendom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bestand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EigendomId = table.Column<int>(type: "int", nullable: true),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrootteInKb = table.Column<int>(type: "int", nullable: false),
                    SoortBestand = table.Column<int>(type: "int", nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bestand_Eigendom_EigendomId",
                        column: x => x.EigendomId,
                        principalTable: "Eigendom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EigenaarEigendom",
                columns: table => new
                {
                    EigenaarId = table.Column<int>(type: "int", nullable: false),
                    EigendomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EigenaarEigendom", x => new { x.EigenaarId, x.EigendomId });
                    table.ForeignKey(
                        name: "FK_EigenaarEigendom_Eigenaar_EigenaarId",
                        column: x => x.EigenaarId,
                        principalTable: "Eigenaar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EigenaarEigendom_Eigendom_EigendomId",
                        column: x => x.EigendomId,
                        principalTable: "Eigendom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Herziening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EigendomId = table.Column<int>(type: "int", nullable: true),
                    Herzieningsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VolgendeHerziening = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Herziening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Herziening_Eigendom_EigendomId",
                        column: x => x.EigendomId,
                        principalTable: "Eigendom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Kadaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EigendomId = table.Column<int>(type: "int", nullable: true),
                    KadastraalNummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deeloppervlakte = table.Column<float>(type: "real", nullable: false),
                    KadastraleGrootte = table.Column<float>(type: "real", nullable: false),
                    ObjectType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kadaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kadaster_Eigendom_EigendomId",
                        column: x => x.EigendomId,
                        principalTable: "Eigendom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Overeenkomst",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EigendomId = table.Column<int>(type: "int", nullable: true),
                    Dossiernummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ingangsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Einddatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grondwaarde = table.Column<float>(type: "real", nullable: false),
                    DatumAkte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rentepercentage = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Overeenkomst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Overeenkomst_Eigendom_EigendomId",
                        column: x => x.EigendomId,
                        principalTable: "Eigendom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Financien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvereenkomstId = table.Column<int>(type: "int", nullable: false),
                    Ingangsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Einddatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bedrag = table.Column<float>(type: "real", nullable: false),
                    FactureringsWijze = table.Column<int>(type: "int", nullable: false),
                    Frequentie = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Financien_Overeenkomst_OvereenkomstId",
                        column: x => x.OvereenkomstId,
                        principalTable: "Overeenkomst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$uJIJxdKuWzAFa/0D9KZTm.GkAu07jCJH9SuDTkDLJI4cO6VBsHCIG");

            migrationBuilder.CreateIndex(
                name: "IX_Adres_EigendomId",
                table: "Adres",
                column: "EigendomId",
                unique: true,
                filter: "[EigendomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bestand_EigendomId",
                table: "Bestand",
                column: "EigendomId");

            migrationBuilder.CreateIndex(
                name: "IX_EigenaarEigendom_EigendomId",
                table: "EigenaarEigendom",
                column: "EigendomId");

            migrationBuilder.CreateIndex(
                name: "IX_Financien_OvereenkomstId",
                table: "Financien",
                column: "OvereenkomstId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Herziening_EigendomId",
                table: "Herziening",
                column: "EigendomId",
                unique: true,
                filter: "[EigendomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kadaster_EigendomId",
                table: "Kadaster",
                column: "EigendomId",
                unique: true,
                filter: "[EigendomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Overeenkomst_EigendomId",
                table: "Overeenkomst",
                column: "EigendomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adres");

            migrationBuilder.DropTable(
                name: "Bestand");

            migrationBuilder.DropTable(
                name: "EigenaarEigendom");

            migrationBuilder.DropTable(
                name: "Financien");

            migrationBuilder.DropTable(
                name: "Herziening");

            migrationBuilder.DropTable(
                name: "Kadaster");

            migrationBuilder.DropTable(
                name: "Eigenaar");

            migrationBuilder.DropTable(
                name: "Overeenkomst");

            migrationBuilder.DropTable(
                name: "Eigendom");

            migrationBuilder.AlterColumn<string>(
                name: "Wachtwoord",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Voornamen",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Emailadres",
                table: "Gebruiker",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$PArrF7Nsejza2sfTHyxyOeRUTqHALFZPGfXGTz/ERXOIcYi9UNPe2");
        }
    }
}
