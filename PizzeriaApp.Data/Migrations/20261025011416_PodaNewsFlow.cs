using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzeriaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class PodaNewsFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Reacciones");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "TemaEtiquetas");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "TiposReaccion");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "Temas");

            migrationBuilder.DropTable(
                name: "Ligas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RolId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RolId",
                keyValue: 2,
                columns: new[] { "Descripcion", "Nombre" },
                values: new object[] { "Cliente de la pizzería", "Usuario" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                    table.ForeignKey(
                        name: "FK_Categorias_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    EtiquetaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.EtiquetaId);
                });

            migrationBuilder.CreateTable(
                name: "Ligas",
                columns: table => new
                {
                    LigaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ligas", x => x.LigaId);
                });

            migrationBuilder.CreateTable(
                name: "TiposReaccion",
                columns: table => new
                {
                    TipoReaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposReaccion", x => x.TipoReaccionId);
                });

            migrationBuilder.CreateTable(
                name: "Temas",
                columns: table => new
                {
                    TemaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temas", x => x.TemaId);
                    table.ForeignKey(
                        name: "FK_Temas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Temas_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Temas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    EquipoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LigaId = table.Column<int>(type: "int", nullable: false),
                    Fundacion = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.EquipoId);
                    table.ForeignKey(
                        name: "FK_Equipos_Ligas_LigaId",
                        column: x => x.LigaId,
                        principalTable: "Ligas",
                        principalColumn: "LigaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reacciones",
                columns: table => new
                {
                    ReaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemaId = table.Column<int>(type: "int", nullable: false),
                    TipoReaccionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reacciones", x => x.ReaccionId);
                    table.ForeignKey(
                        name: "FK_Reacciones_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "TemaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reacciones_TiposReaccion_TipoReaccionId",
                        column: x => x.TipoReaccionId,
                        principalTable: "TiposReaccion",
                        principalColumn: "TipoReaccionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reacciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    ReporteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    TemaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaReporte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.ReporteId);
                    table.ForeignKey(
                        name: "FK_Reportes_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reportes_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "TemaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reportes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    RespuestaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    TemaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.RespuestaId);
                    table.ForeignKey(
                        name: "FK_Respuestas_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Respuestas_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "TemaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Respuestas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemaEtiquetas",
                columns: table => new
                {
                    TemaEtiquetaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtiquetaId = table.Column<int>(type: "int", nullable: false),
                    TemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemaEtiquetas", x => x.TemaEtiquetaId);
                    table.ForeignKey(
                        name: "FK_TemaEtiquetas_Etiquetas_EtiquetaId",
                        column: x => x.EtiquetaId,
                        principalTable: "Etiquetas",
                        principalColumn: "EtiquetaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemaEtiquetas_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "TemaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    JugadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Posicion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.JugadorId);
                    table.ForeignKey(
                        name: "FK_Jugadores_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaId", "Descripcion", "EstadoId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Debates sobre la Champions League", 1, "Champions League" },
                    { 2, "Foro sobre f�tbol ingl�s", 1, "Premier League" },
                    { 3, "Foro sobre f�tbol de Costa Rica", 1, "Liga Promerica" }
                });

            migrationBuilder.InsertData(
                table: "Etiquetas",
                columns: new[] { "EtiquetaId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Fichajes" },
                    { 2, "VAR" },
                    { 3, "Cl�sico" }
                });

            migrationBuilder.InsertData(
                table: "Ligas",
                columns: new[] { "LigaId", "Nombre", "Pais" },
                values: new object[,]
                {
                    { 1, "La Liga", "Espa�a" },
                    { 2, "Premier League", "Inglaterra" },
                    { 3, "Liga Promerica", "Costa Rica" }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RolId",
                keyValue: 2,
                columns: new[] { "Descripcion", "Nombre" },
                values: new object[] { "Gestiona contenido del foro", "Moderador" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RolId", "Descripcion", "Nombre" },
                values: new object[] { 3, "Usuario general del foro", "Usuario" });

            migrationBuilder.InsertData(
                table: "TiposReaccion",
                columns: new[] { "TipoReaccionId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Me gusta" },
                    { 2, "Golazo" },
                    { 3, "Pol�mico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_EstadoId",
                table: "Categorias",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_LigaId",
                table: "Equipos",
                column: "LigaId");

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_EquipoId",
                table: "Jugadores",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_TemaId",
                table: "Reacciones",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_TipoReaccionId",
                table: "Reacciones",
                column: "TipoReaccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_UsuarioId_TemaId_TipoReaccionId",
                table: "Reacciones",
                columns: new[] { "UsuarioId", "TemaId", "TipoReaccionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_EstadoId",
                table: "Reportes",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_TemaId",
                table: "Reportes",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_UsuarioId",
                table: "Reportes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_EstadoId",
                table: "Respuestas",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_TemaId",
                table: "Respuestas",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_UsuarioId",
                table: "Respuestas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TemaEtiquetas_EtiquetaId",
                table: "TemaEtiquetas",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_TemaEtiquetas_TemaId_EtiquetaId",
                table: "TemaEtiquetas",
                columns: new[] { "TemaId", "EtiquetaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Temas_CategoriaId",
                table: "Temas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Temas_EstadoId",
                table: "Temas",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Temas_UsuarioId",
                table: "Temas",
                column: "UsuarioId");
        }
    }
}
