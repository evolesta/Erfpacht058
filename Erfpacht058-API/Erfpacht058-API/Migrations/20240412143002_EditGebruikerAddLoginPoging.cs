using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class EditGebruikerAddLoginPoging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogingPoging",
                table: "Gebruiker",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LogingPoging", "Wachtwoord" },
                values: new object[] { 0, "$2a$11$Z8YcmKs6xz3hPbHRTNDVUOq60htPap6TAi1syz8FWGf1sUr4CIREK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogingPoging",
                table: "Gebruiker");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$6DkF7BUijzRSiPxvKgDQ2uy9EAUNeheheP68uMSnH6zx6d.3hFDiy");
        }
    }
}
