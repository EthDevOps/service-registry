using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class dpo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControllerId",
                table: "GdprRegisters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DpoOrganisationId",
                table: "GdprRegisters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GdprController",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GdprController", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GdprDpoOrganisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GdprDpoOrganisation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GdprRegisters_ControllerId",
                table: "GdprRegisters",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_GdprRegisters_DpoOrganisationId",
                table: "GdprRegisters",
                column: "DpoOrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GdprRegisters_GdprController_ControllerId",
                table: "GdprRegisters",
                column: "ControllerId",
                principalTable: "GdprController",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GdprRegisters_GdprDpoOrganisation_DpoOrganisationId",
                table: "GdprRegisters",
                column: "DpoOrganisationId",
                principalTable: "GdprDpoOrganisation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GdprRegisters_GdprController_ControllerId",
                table: "GdprRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_GdprRegisters_GdprDpoOrganisation_DpoOrganisationId",
                table: "GdprRegisters");

            migrationBuilder.DropTable(
                name: "GdprController");

            migrationBuilder.DropTable(
                name: "GdprDpoOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_GdprRegisters_ControllerId",
                table: "GdprRegisters");

            migrationBuilder.DropIndex(
                name: "IX_GdprRegisters_DpoOrganisationId",
                table: "GdprRegisters");

            migrationBuilder.DropColumn(
                name: "ControllerId",
                table: "GdprRegisters");

            migrationBuilder.DropColumn(
                name: "DpoOrganisationId",
                table: "GdprRegisters");
        }
    }
}
