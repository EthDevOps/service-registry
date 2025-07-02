using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class MoveHostingInformationToCatalogService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnpremiseHosts_HostingInfo_HostingInformationId",
                table: "OnpremiseHosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_HostingInfo_HostingId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "HostingInfo");

            migrationBuilder.DropIndex(
                name: "IX_Services_HostingId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_OnpremiseHosts_HostingInformationId",
                table: "OnpremiseHosts");

            migrationBuilder.DropColumn(
                name: "HostingInformationId",
                table: "OnpremiseHosts");

            migrationBuilder.RenameColumn(
                name: "HostingId",
                table: "Services",
                newName: "HostingType");

            migrationBuilder.AddColumn<string>(
                name: "HostingCountry",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SaasRegionReference",
                table: "Services",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatalogServiceId",
                table: "OnpremiseHosts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OnpremiseHosts_CatalogServiceId",
                table: "OnpremiseHosts",
                column: "CatalogServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnpremiseHosts_Services_CatalogServiceId",
                table: "OnpremiseHosts",
                column: "CatalogServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnpremiseHosts_Services_CatalogServiceId",
                table: "OnpremiseHosts");

            migrationBuilder.DropIndex(
                name: "IX_OnpremiseHosts_CatalogServiceId",
                table: "OnpremiseHosts");

            migrationBuilder.DropColumn(
                name: "HostingCountry",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SaasRegionReference",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CatalogServiceId",
                table: "OnpremiseHosts");

            migrationBuilder.RenameColumn(
                name: "HostingType",
                table: "Services",
                newName: "HostingId");

            migrationBuilder.AddColumn<int>(
                name: "HostingInformationId",
                table: "OnpremiseHosts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HostingInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HostingCountry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HostingType = table.Column<int>(type: "integer", nullable: false),
                    SaasRegionReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostingInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_HostingId",
                table: "Services",
                column: "HostingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnpremiseHosts_HostingInformationId",
                table: "OnpremiseHosts",
                column: "HostingInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnpremiseHosts_HostingInfo_HostingInformationId",
                table: "OnpremiseHosts",
                column: "HostingInformationId",
                principalTable: "HostingInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_HostingInfo_HostingId",
                table: "Services",
                column: "HostingId",
                principalTable: "HostingInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
