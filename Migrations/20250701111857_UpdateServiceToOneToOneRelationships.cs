using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceToOneToOneRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_GdprRegisterId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_HostingId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_LicenseId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_LifecycleId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_SubscriptionId",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Services_GdprRegisterId",
                table: "Services",
                column: "GdprRegisterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_HostingId",
                table: "Services",
                column: "HostingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_LicenseId",
                table: "Services",
                column: "LicenseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_LifecycleId",
                table: "Services",
                column: "LifecycleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_SubscriptionId",
                table: "Services",
                column: "SubscriptionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_GdprRegisterId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_HostingId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_LicenseId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_LifecycleId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_SubscriptionId",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Services_GdprRegisterId",
                table: "Services",
                column: "GdprRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_HostingId",
                table: "Services",
                column: "HostingId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_LicenseId",
                table: "Services",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_LifecycleId",
                table: "Services",
                column: "LifecycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SubscriptionId",
                table: "Services",
                column: "SubscriptionId");
        }
    }
}
