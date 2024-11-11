using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertyHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure PropertyHistory column is removed if it exists
            migrationBuilder.Sql("ALTER TABLE users DROP COLUMN IF EXISTS PropertyHistory;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Optionally, you can re-add the column if needed
            migrationBuilder.AddColumn<string>(
                name: "PropertyHistory",
                table: "users",
                type: "text",
                nullable: true);
        }
    }
}
