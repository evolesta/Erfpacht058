using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class AddGebruikersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gebruiker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Voornamen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emailadres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Actief = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruiker", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Gebruiker",
                columns: new[] { "Id", "Actief", "Emailadres", "Naam", "Role", "Voornamen", "Wachtwoord" },
                values: new object[] { 1, true, "test@gebruiker.nl", "Gebruiker", 1, "Eerste", "$2a$11$PArrF7Nsejza2sfTHyxyOeRUTqHALFZPGfXGTz/ERXOIcYi9UNPe2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gebruiker");
        }
    }
}
