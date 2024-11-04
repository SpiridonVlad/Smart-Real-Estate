using Domain.Types;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VladCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "HasBalcony",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "HasGarage",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "HasGarden",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "HasPool",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "IsApartament",
                table: "properties");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "properties",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "rating",
                table: "users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "verified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "properties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Dictionary<PropertyFeatureType, int>>(
                name: "Features_Features",
                table: "properties",
                type: "jsonb",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AdditionalInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_images_properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_properties_AddressId",
                table: "properties",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_images_PropertyId",
                table: "images",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_properties_addresses_AddressId",
                table: "properties",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_properties_addresses_AddressId",
                table: "properties");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropIndex(
                name: "IX_properties_AddressId",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "users");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "users");

            migrationBuilder.DropColumn(
                name: "verified",
                table: "users");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "Features_Features",
                table: "properties");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "properties",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "properties",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasBalcony",
                table: "properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGarage",
                table: "properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGarden",
                table: "properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPool",
                table: "properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApartament",
                table: "properties",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
