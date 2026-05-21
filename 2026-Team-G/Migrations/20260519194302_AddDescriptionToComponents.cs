using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_Team_G.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "componentClass",
                table: "Components",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "componentClass",
                table: "Components");
        }
    }
}
