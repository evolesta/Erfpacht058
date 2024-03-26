using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erfpacht058_API.Migrations
{
    /// <inheritdoc />
    public partial class EditOvereenkomstNullableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rentepercentage",
                table: "Overeenkomst",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Grondwaarde",
                table: "Overeenkomst",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Einddatum",
                table: "Overeenkomst",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumAkte",
                table: "Overeenkomst",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$ApMMa.vvGc6.u1mjrD1mYuibT2bL3HgztS1hLhYrcKVT13BJjOL3W");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rentepercentage",
                table: "Overeenkomst",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Grondwaarde",
                table: "Overeenkomst",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Einddatum",
                table: "Overeenkomst",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatumAkte",
                table: "Overeenkomst",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Gebruiker",
                keyColumn: "Id",
                keyValue: 1,
                column: "Wachtwoord",
                value: "$2a$11$bZsjEqm5qrUZrJvTmje33e/RaVW3.G6Bm6RJzmteOtDE5Sj6XgVFq");
        }
    }
}
