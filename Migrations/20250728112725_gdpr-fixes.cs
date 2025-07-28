using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class gdprfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GdprRegisters_GdprController_ControllerId",
                table: "GdprRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_GdprRegisters_GdprDpoOrganisation_DpoOrganisationId",
                table: "GdprRegisters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GdprDpoOrganisation",
                table: "GdprDpoOrganisation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GdprController",
                table: "GdprController");

            migrationBuilder.RenameTable(
                name: "GdprDpoOrganisation",
                newName: "GdprDpoOrganisations");

            migrationBuilder.RenameTable(
                name: "GdprController",
                newName: "GdprControllers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GdprDpoOrganisations",
                table: "GdprDpoOrganisations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GdprControllers",
                table: "GdprControllers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GdprRegisters_GdprControllers_ControllerId",
                table: "GdprRegisters",
                column: "ControllerId",
                principalTable: "GdprControllers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GdprRegisters_GdprDpoOrganisations_DpoOrganisationId",
                table: "GdprRegisters",
                column: "DpoOrganisationId",
                principalTable: "GdprDpoOrganisations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GdprRegisters_GdprControllers_ControllerId",
                table: "GdprRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_GdprRegisters_GdprDpoOrganisations_DpoOrganisationId",
                table: "GdprRegisters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GdprDpoOrganisations",
                table: "GdprDpoOrganisations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GdprControllers",
                table: "GdprControllers");

            migrationBuilder.RenameTable(
                name: "GdprDpoOrganisations",
                newName: "GdprDpoOrganisation");

            migrationBuilder.RenameTable(
                name: "GdprControllers",
                newName: "GdprController");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GdprDpoOrganisation",
                table: "GdprDpoOrganisation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GdprController",
                table: "GdprController",
                column: "Id");

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
    }
}
