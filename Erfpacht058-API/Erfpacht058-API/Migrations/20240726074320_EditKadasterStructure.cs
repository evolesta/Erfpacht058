using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class EditKadasterStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ObjectType",
                table: "Kadaster",
                newName: "Gebruiksdoel");

            migrationBuilder.RenameColumn(
                name: "KadastraleGrootte",
                table: "Kadaster",
                newName: "Oppervlakte");

            migrationBuilder.RenameColumn(
                name: "KadastraalNummer",
                table: "Kadaster",
                newName: "BAGID");

            migrationBuilder.RenameColumn(
                name: "Deeloppervlakte",
                table: "Kadaster",
                newName: "Bouwjaar");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$Uc5.4aivaWigaWNIFNC7ceQmgjlxu1kXf71Rf3CRiVBlgh.ZTKcaq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Oppervlakte",
                table: "Kadaster",
                newName: "KadastraleGrootte");

            migrationBuilder.RenameColumn(
                name: "Gebruiksdoel",
                table: "Kadaster",
                newName: "ObjectType");

            migrationBuilder.RenameColumn(
                name: "Bouwjaar",
                table: "Kadaster",
                newName: "Deeloppervlakte");

            migrationBuilder.RenameColumn(
                name: "BAGID",
                table: "Kadaster",
                newName: "KadastraalNummer");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$didHBqcx3BOvAVvl5izQHOwU37giXZ3yWHOVTJTlR5vAJp3Enb0fu");
        }
    }
}
