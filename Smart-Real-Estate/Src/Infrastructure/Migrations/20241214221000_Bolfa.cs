using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Bolfa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Features_Features",
                table: "properties",
                newName: "features");

            migrationBuilder.RenameColumn(
                name: "Features_Features",
                table: "listings",
                newName: "features");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "features",
                table: "properties",
                newName: "Features_Features");

            migrationBuilder.RenameColumn(
                name: "features",
                table: "listings",
                newName: "Features_Features");
        }
    }
}
