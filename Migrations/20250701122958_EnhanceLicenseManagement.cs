using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuokkaServiceRegistry.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceLicenseManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cvv",
                table: "BillingInfo");

            migrationBuilder.AlterColumn<string>(
                name: "LicenseUrl",
                table: "Licenses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "CompatibilityLevel",
                table: "Licenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Licenses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Licenses",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCopyleft",
                table: "Licenses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOsiApproved",
                table: "Licenses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresAttribution",
                table: "Licenses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresSourceDisclosure",
                table: "Licenses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Licenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Licenses",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompatibilityLevel",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IsCopyleft",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IsOsiApproved",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "RequiresAttribution",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "RequiresSourceDisclosure",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Licenses");

            migrationBuilder.AlterColumn<string>(
                name: "LicenseUrl",
                table: "Licenses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cvv",
                table: "BillingInfo",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);
        }
    }
}
