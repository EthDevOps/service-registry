using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class IntegrateAddressIntoVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_AddressId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Vendors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Vendors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Vendors");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Vendors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_AddressId",
                table: "Vendors",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
