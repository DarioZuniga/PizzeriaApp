using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoConfirmacionReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoConfirmacion",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoConfirmacion",
                table: "Reservas");
        }
    }
}
