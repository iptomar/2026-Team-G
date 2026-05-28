using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_Team_G.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorUserNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formularios_Utilizadores_UtilizadorId",
                table: "Formularios");

            migrationBuilder.DropIndex(
                name: "IX_Formularios_UtilizadorId",
                table: "Formularios");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Formularios");

            migrationBuilder.AddColumn<string>(
                name: "CreatorUserName",
                table: "Formularios",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorUserName",
                table: "Formularios");

            migrationBuilder.AddColumn<int>(
                name: "UtilizadorId",
                table: "Formularios",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formularios_UtilizadorId",
                table: "Formularios",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formularios_Utilizadores_UtilizadorId",
                table: "Formularios",
                column: "UtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "Id");
        }
    }
}
