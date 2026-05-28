using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_Team_G.Migrations
{
    /// <inheritdoc />
    public partial class AddHeightToFormFieldModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "FormFieldModels",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "FormFieldModels");
        }
    }
}
