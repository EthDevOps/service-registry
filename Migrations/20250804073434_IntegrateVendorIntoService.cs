using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class IntegrateVendorIntoService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnpremiseHosts_Vendors_CloudProviderId",
                table: "OnpremiseHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Vendors_VendorId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Services_VendorId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_OnpremiseHosts_CloudProviderId",
                table: "OnpremiseHosts");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CloudProviderId",
                table: "OnpremiseHosts");

            migrationBuilder.AddColumn<int>(
                name: "BillingInformationId",
                table: "Services",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Services",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCity",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCountry",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorName",
                table: "Services",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorWebsiteUrl",
                table: "Services",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CloudProvider",
                table: "OnpremiseHosts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Services_BillingInformationId",
                table: "Services",
                column: "BillingInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PaymentMethodId",
                table: "Services",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_BillingInfo_BillingInformationId",
                table: "Services",
                column: "BillingInformationId",
                principalTable: "BillingInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_PaymentMethods_PaymentMethodId",
                table: "Services",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_BillingInfo_BillingInformationId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_PaymentMethods_PaymentMethodId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_BillingInformationId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_PaymentMethodId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "BillingInformationId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "VendorCity",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "VendorCountry",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "VendorName",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "VendorWebsiteUrl",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CloudProvider",
                table: "OnpremiseHosts");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CloudProviderId",
                table: "OnpremiseHosts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BillingInformationId = table.Column<int>(type: "integer", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_BillingInfo_BillingInformationId",
                        column: x => x.BillingInformationId,
                        principalTable: "BillingInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vendors_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_VendorId",
                table: "Services",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_OnpremiseHosts_CloudProviderId",
                table: "OnpremiseHosts",
                column: "CloudProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_BillingInformationId",
                table: "Vendors",
                column: "BillingInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_PaymentMethodId",
                table: "Vendors",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnpremiseHosts_Vendors_CloudProviderId",
                table: "OnpremiseHosts",
                column: "CloudProviderId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Vendors_VendorId",
                table: "Services",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
