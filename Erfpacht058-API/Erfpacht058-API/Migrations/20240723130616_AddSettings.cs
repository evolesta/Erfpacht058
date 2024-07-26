using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class AddSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BAGAPI = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$didHBqcx3BOvAVvl5izQHOwU37giXZ3yWHOVTJTlR5vAJp3Enb0fu");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "BAGAPI" },
                values: new object[] { 1, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$SntTFq0Tb38YnUySOTtMpeCX.8FLxSnz/z4HuDi9//qNOw9RBKl3y");
        }
    }
}
