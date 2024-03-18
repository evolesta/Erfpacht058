using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class EditKadasterAddLaatsteSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LaatsteSynchronisatie",
                table: "Kadaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$XJSlCEqoKKfEEc8jyU/gDOriA3796G3R25Do6p0gLBSO1zyz4SB9a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaatsteSynchronisatie",
                table: "Kadaster");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$kOE.FcxI00N2lAvFHx0Am.cnH6XS5CxQ8A8RE9Bu8ayUblliRe1N.");
        }
    }
}
