using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublicToCatalogService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Services",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Services");
        }
    }
}
