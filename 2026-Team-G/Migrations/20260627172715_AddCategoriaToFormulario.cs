using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_Team_G.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriaToFormulario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Formularios",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formularios_CategoriaId",
                table: "Formularios",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formularios_Categorias_CategoriaId",
                table: "Formularios",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formularios_Categorias_CategoriaId",
                table: "Formularios");

            migrationBuilder.DropIndex(
                name: "IX_Formularios_CategoriaId",
                table: "Formularios");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Formularios");
        }
    }
}
