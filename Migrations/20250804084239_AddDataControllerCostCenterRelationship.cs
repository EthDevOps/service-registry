using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class AddDataControllerCostCenterRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                table: "GdprControllers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "GdprControllers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GdprControllers_CostCenterId",
                table: "GdprControllers",
                column: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_GdprControllers_CostCenters_CostCenterId",
                table: "GdprControllers",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GdprControllers_CostCenters_CostCenterId",
                table: "GdprControllers");

            migrationBuilder.DropIndex(
                name: "IX_GdprControllers_CostCenterId",
                table: "GdprControllers");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                table: "GdprControllers");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "GdprControllers");
        }
    }
}
