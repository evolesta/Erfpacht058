using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class EditTemplateAddFiltersRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filters",
                table: "Template");

            migrationBuilder.CreateTable(
                name: "Filter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filter_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$SOlz6ulWACMQo6xrytW6MuOcZ0bTE/XplzVKw2CPm14ZaG3IFaHAK");

            migrationBuilder.CreateIndex(
                name: "IX_Filter_TemplateId",
                table: "Filter",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filter");

            migrationBuilder.AddColumn<string>(
                name: "Filters",
                table: "Template",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$SFtGvsdL7IZ6Wm..I3DSreEJlCfMztpOyVkehh7cdd.RLjbwEibX2");
        }
    }
}
