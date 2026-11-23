using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDiasCierre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiasCierre",
                table: "ConfiguracionesReserva",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiasCierre",
                table: "ConfiguracionesReserva");
        }
    }
}
