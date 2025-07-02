using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVendorForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_BillingInfo_BillingInformationId",
                table: "Vendors");

            migrationBuilder.AlterColumn<int>(
                name: "BillingInformationId",
                table: "Vendors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Vendors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_BillingInfo_BillingInformationId",
                table: "Vendors",
                column: "BillingInformationId",
                principalTable: "BillingInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_BillingInfo_BillingInformationId",
                table: "Vendors");

            migrationBuilder.AlterColumn<int>(
                name: "BillingInformationId",
                table: "Vendors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Vendors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Addresses_AddressId",
                table: "Vendors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_BillingInfo_BillingInformationId",
                table: "Vendors",
                column: "BillingInformationId",
                principalTable: "BillingInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
