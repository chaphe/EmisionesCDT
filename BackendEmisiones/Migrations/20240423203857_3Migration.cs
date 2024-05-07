using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendEmisiones.Migrations
{
    /// <inheritdoc />
    public partial class _3Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaDeteccion",
                table: "EmisionesFugitivas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ReporteMensualGas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlantaId = table.Column<int>(type: "int", nullable: false),
                    Planta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GasId = table.Column<int>(type: "int", nullable: false),
                    Gas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anho = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteMensualGas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReporteMensualGasDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SistemaId = table.Column<int>(type: "int", nullable: false),
                    Sistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enision = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmisionCO2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReporteMensualGasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteMensualGasDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReporteMensualGasDetalle_ReporteMensualGas_ReporteMensualGasId",
                        column: x => x.ReporteMensualGasId,
                        principalTable: "ReporteMensualGas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReporteMensualGasDetalle_ReporteMensualGasId",
                table: "ReporteMensualGasDetalle",
                column: "ReporteMensualGasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReporteMensualGasDetalle");

            migrationBuilder.DropTable(
                name: "ReporteMensualGas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaDeteccion",
                table: "EmisionesFugitivas",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
