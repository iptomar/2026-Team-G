using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_Team_G.Migrations
{
    /// <inheritdoc />
    public partial class AddWidthToFormFieldModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "FormFieldModels",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Width",
                table: "FormFieldModels");
        }
    }
}
