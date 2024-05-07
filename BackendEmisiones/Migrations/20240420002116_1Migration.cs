using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendEmisiones.Migrations
{
    /// <inheritdoc />
    public partial class _1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComposicionesGas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreGas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Oxigeno = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Nitrogeno = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Metano = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DioxidoCarbono = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Etano = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Propano = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Isobutano = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Nbutano = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Isopentano = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Npentano = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NhexanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NheptanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NoctanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NnonanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NdecanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NundecanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NdodecanoIsomeros = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposicionesGas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naturaleza = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CargoContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactorGwp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReporteMensual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlantaId = table.Column<int>(type: "int", nullable: false),
                    Planta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anho = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteMensual", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoFuente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClasificacion = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoEmision = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoFuente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FactoresEmision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreGas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorCo2combustion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorCh4fugitivas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorCo2fugitivas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComposicionGasId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoresEmision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoresEmision_ComposicionesGas_ComposicionGasId",
                        column: x => x.ComposicionGasId,
                        principalTable: "ComposicionesGas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Plantas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plantas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReporteMensualDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorEmisionId = table.Column<int>(type: "int", nullable: false),
                    NombreGas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SistemaId = table.Column<int>(type: "int", nullable: false),
                    Sistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enision = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmisionCO2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReporteMensualId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteMensualDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReporteMensualDetalle_ReporteMensual_ReporteMensualId",
                        column: x => x.ReporteMensualId,
                        principalTable: "ReporteMensual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sistemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlantaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sistemas_Plantas_PlantaId",
                        column: x => x.PlantaId,
                        principalTable: "Plantas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmisionesCombustion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Consecutivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HorasOperacion = table.Column<int>(type: "int", nullable: false),
                    EficienciaCombustion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SistemaId = table.Column<int>(type: "int", nullable: false),
                    TipoFuenteId = table.Column<int>(type: "int", nullable: false),
                    FactorEmisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmisionesCombustion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmisionesCombustion_FactoresEmision_FactorEmisionId",
                        column: x => x.FactorEmisionId,
                        principalTable: "FactoresEmision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmisionesCombustion_Sistemas_SistemaId",
                        column: x => x.SistemaId,
                        principalTable: "Sistemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmisionesCombustion_TipoFuente_TipoFuenteId",
                        column: x => x.TipoFuenteId,
                        principalTable: "TipoFuente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmisionesFugitivas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorEmisionId = table.Column<int>(type: "int", nullable: false),
                    Consecutivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HorasOperacion = table.Column<int>(type: "int", nullable: false),
                    Tamano = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Presion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Temperatura = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Fuga = table.Column<bool>(type: "bit", nullable: false),
                    CaudalEmision = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FactorGwp = table.Column<int>(type: "int", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDeteccion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaReparacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SistemaId = table.Column<int>(type: "int", nullable: false),
                    TipoFuenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmisionesFugitivas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmisionesFugitivas_Sistemas_SistemaId",
                        column: x => x.SistemaId,
                        principalTable: "Sistemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmisionesFugitivas_TipoFuente_TipoFuenteId",
                        column: x => x.TipoFuenteId,
                        principalTable: "TipoFuente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evidencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioDeteccionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDeteccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FotoAntes = table.Column<bool>(type: "bit", nullable: false),
                    Video = table.Column<bool>(type: "bit", nullable: false),
                    FechaReparacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FotoDespues = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuarioReparacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmisionFugitivaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evidencias_EmisionesFugitivas_EmisionFugitivaId",
                        column: x => x.EmisionFugitivaId,
                        principalTable: "EmisionesFugitivas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmisionesCombustion_FactorEmisionId",
                table: "EmisionesCombustion",
                column: "FactorEmisionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmisionesCombustion_SistemaId",
                table: "EmisionesCombustion",
                column: "SistemaId");

            migrationBuilder.CreateIndex(
                name: "IX_EmisionesCombustion_TipoFuenteId",
                table: "EmisionesCombustion",
                column: "TipoFuenteId");

            migrationBuilder.CreateIndex(
                name: "IX_EmisionesFugitivas_SistemaId",
                table: "EmisionesFugitivas",
                column: "SistemaId");

            migrationBuilder.CreateIndex(
                name: "IX_EmisionesFugitivas_TipoFuenteId",
                table: "EmisionesFugitivas",
                column: "TipoFuenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidencias_EmisionFugitivaId",
                table: "Evidencias",
                column: "EmisionFugitivaId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoresEmision_ComposicionGasId",
                table: "FactoresEmision",
                column: "ComposicionGasId");

            migrationBuilder.CreateIndex(
                name: "IX_Plantas_EmpresaId",
                table: "Plantas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteMensualDetalle_ReporteMensualId",
                table: "ReporteMensualDetalle",
                column: "ReporteMensualId");

            migrationBuilder.CreateIndex(
                name: "IX_Sistemas_PlantaId",
                table: "Sistemas",
                column: "PlantaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmisionesCombustion");

            migrationBuilder.DropTable(
                name: "Evidencias");

            migrationBuilder.DropTable(
                name: "ReporteMensualDetalle");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "FactoresEmision");

            migrationBuilder.DropTable(
                name: "EmisionesFugitivas");

            migrationBuilder.DropTable(
                name: "ReporteMensual");

            migrationBuilder.DropTable(
                name: "ComposicionesGas");

            migrationBuilder.DropTable(
                name: "Sistemas");

            migrationBuilder.DropTable(
                name: "TipoFuente");

            migrationBuilder.DropTable(
                name: "Plantas");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
