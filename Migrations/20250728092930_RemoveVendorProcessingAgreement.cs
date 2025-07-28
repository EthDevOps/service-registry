using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVendorProcessingAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GdprProcessingAgreementExists",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ProcessingAgreementLink",
                table: "Vendors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GdprProcessingAgreementExists",
                table: "Vendors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProcessingAgreementLink",
                table: "Vendors",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
