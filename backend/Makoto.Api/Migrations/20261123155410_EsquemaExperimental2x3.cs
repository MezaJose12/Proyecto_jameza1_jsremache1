using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Makoto.Api.Migrations
{
    /// <inheritdoc />
    public partial class EsquemaExperimental2x3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "condiciones_vocales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NivelFrecuencia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FrecuenciaBaseHz = table.Column<double>(type: "double precision", nullable: false),
                    RutaAudioRelativa = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condiciones_vocales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sesiones_experimentales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoParticipante = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Modalidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConsentimientoAceptado = table.Column<bool>(type: "boolean", nullable: false),
                    CondicionVocalId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaInicioUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sesiones_experimentales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sesiones_experimentales_condiciones_vocales_CondicionVocalId",
                        column: x => x.CondicionVocalId,
                        principalTable: "condiciones_vocales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluaciones_empatia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SesionExperimentalId = table.Column<Guid>(type: "uuid", nullable: false),
                    PetsItem1 = table.Column<int>(type: "integer", nullable: false),
                    PetsItem2 = table.Column<int>(type: "integer", nullable: false),
                    PetsItem3 = table.Column<int>(type: "integer", nullable: false),
                    PetsItem4 = table.Column<int>(type: "integer", nullable: false),
                    PetsItem5 = table.Column<int>(type: "integer", nullable: false),
                    CalidezPercibida = table.Column<int>(type: "integer", nullable: false),
                    NaturalidadVoz = table.Column<int>(type: "integer", nullable: false),
                    GeneroPercibido = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TonoPercibido = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ComentarioCualitativo = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FechaRegistroUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluaciones_empatia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evaluaciones_empatia_sesiones_experimentales_SesionExperime~",
                        column: x => x.SesionExperimentalId,
                        principalTable: "sesiones_experimentales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "eventos_telemetria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SesionExperimentalId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEvento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Turno = table.Column<int>(type: "integer", nullable: true),
                    DuracionSegundos = table.Column<double>(type: "double precision", nullable: true),
                    DetalleJson = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TimestampUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventos_telemetria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_eventos_telemetria_sesiones_experimentales_SesionExperiment~",
                        column: x => x.SesionExperimentalId,
                        principalTable: "sesiones_experimentales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "condiciones_vocales",
                columns: new[] { "Id", "Codigo", "FrecuenciaBaseHz", "Genero", "NivelFrecuencia", "RutaAudioRelativa" },
                values: new object[,]
                {
                    { 1, "F_GRAVE", 165.0, "Femenino", "Grave", "f_grave.wav" },
                    { 2, "F_NEUTRA", 210.0, "Femenino", "Neutra", "f_neutra.wav" },
                    { 3, "F_AGUDA", 255.0, "Femenino", "Aguda", "f_aguda.wav" },
                    { 4, "M_GRAVE", 100.0, "Masculino", "Grave", "m_grave.wav" },
                    { 5, "M_NEUTRA", 130.0, "Masculino", "Neutra", "m_neutra.wav" },
                    { 6, "M_AGUDA", 165.0, "Masculino", "Aguda", "m_aguda.wav" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_condiciones_vocales_Codigo",
                table: "condiciones_vocales",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluaciones_empatia_SesionExperimentalId",
                table: "evaluaciones_empatia",
                column: "SesionExperimentalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_eventos_telemetria_SesionExperimentalId",
                table: "eventos_telemetria",
                column: "SesionExperimentalId");

            migrationBuilder.CreateIndex(
                name: "IX_sesiones_experimentales_CondicionVocalId",
                table: "sesiones_experimentales",
                column: "CondicionVocalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "evaluaciones_empatia");

            migrationBuilder.DropTable(
                name: "eventos_telemetria");

            migrationBuilder.DropTable(
                name: "sesiones_experimentales");

            migrationBuilder.DropTable(
                name: "condiciones_vocales");
        }
    }
}
