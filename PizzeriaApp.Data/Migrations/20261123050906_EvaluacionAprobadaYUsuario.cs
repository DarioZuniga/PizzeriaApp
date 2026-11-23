using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class EvaluacionAprobadaYUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aprobada",
                table: "Evaluaciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Evaluaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluaciones_UsuarioId",
                table: "Evaluaciones",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluaciones_Usuarios_UsuarioId",
                table: "Evaluaciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluaciones_Usuarios_UsuarioId",
                table: "Evaluaciones");

            migrationBuilder.DropIndex(
                name: "IX_Evaluaciones_UsuarioId",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "Aprobada",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Evaluaciones");
        }
    }
}
