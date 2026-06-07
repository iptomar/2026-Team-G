using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_Team_G.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissoesAndRespostas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Submissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FormularioId = table.Column<int>(type: "INTEGER", nullable: false),
                    UtilizadorId = table.Column<string>(type: "TEXT", nullable: false),
                    DataSubmissao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissoes_AspNetUsers_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissoes_Formularios_FormularioId",
                        column: x => x.FormularioId,
                        principalTable: "Formularios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respostas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubmissaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    FormFieldModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Valor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respostas_FormFieldModels_FormFieldModelId",
                        column: x => x.FormFieldModelId,
                        principalTable: "FormFieldModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Respostas_Submissoes_SubmissaoId",
                        column: x => x.SubmissaoId,
                        principalTable: "Submissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Respostas_FormFieldModelId",
                table: "Respostas",
                column: "FormFieldModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Respostas_SubmissaoId",
                table: "Respostas",
                column: "SubmissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissoes_FormularioId",
                table: "Submissoes",
                column: "FormularioId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissoes_UtilizadorId",
                table: "Submissoes",
                column: "UtilizadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Respostas");

            migrationBuilder.DropTable(
                name: "Submissoes");
        }
    }
}
