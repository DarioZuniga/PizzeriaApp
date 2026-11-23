using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RespuestaAdminEvaluacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRespuesta",
                table: "Evaluaciones",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespuestaAdmin",
                table: "Evaluaciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRespuesta",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "RespuestaAdmin",
                table: "Evaluaciones");
        }
    }
}
