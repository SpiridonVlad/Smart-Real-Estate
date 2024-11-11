using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "users");

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
                name: "Rooms",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "Surface",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "listings");

            migrationBuilder.DropColumn(
                name: "IsHighlighted",
                table: "listings");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "listings");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "properties",
                newName: "Features");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "PropertyHistory",
                table: "users",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "properties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "listings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int[]>(
                name: "Properties",
                table: "listings",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_properties_Address_Id",
                table: "properties",
                column: "Id",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_properties_Address_Id",
                table: "properties");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropColumn(
                name: "PropertyHistory",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "users");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "listings");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "listings");

            migrationBuilder.RenameColumn(
                name: "Features",
                table: "properties",
                newName: "Address");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<int>(
                name: "Rooms",
                table: "properties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Surface",
                table: "properties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "listings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHighlighted",
                table: "listings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "listings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
