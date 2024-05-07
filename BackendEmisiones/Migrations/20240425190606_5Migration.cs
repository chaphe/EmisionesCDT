using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendEmisiones.Migrations
{
    /// <inheritdoc />
    public partial class _5Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmisionesFugitivas_FactorEmisionId",
                table: "EmisionesFugitivas",
                column: "FactorEmisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmisionesFugitivas_FactoresEmision_FactorEmisionId",
                table: "EmisionesFugitivas",
                column: "FactorEmisionId",
                principalTable: "FactoresEmision",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmisionesFugitivas_FactoresEmision_FactorEmisionId",
                table: "EmisionesFugitivas");

            migrationBuilder.DropIndex(
                name: "IX_EmisionesFugitivas_FactorEmisionId",
                table: "EmisionesFugitivas");
        }
    }
}
