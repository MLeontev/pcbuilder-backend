using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PowerConnectorCompatibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PowerConnectorCompatibilities",
                columns: table => new
                {
                    SourceConnectorId = table.Column<int>(type: "integer", nullable: false),
                    CompatibleConnectorId = table.Column<int>(type: "integer", nullable: false),
                    RequiredQuantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerConnectorCompatibilities", x => new { x.SourceConnectorId, x.CompatibleConnectorId });
                    table.ForeignKey(
                        name: "FK_PowerConnectorCompatibilities_PowerConnectors_CompatibleCon~",
                        column: x => x.CompatibleConnectorId,
                        principalTable: "PowerConnectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerConnectorCompatibilities_PowerConnectors_SourceConnect~",
                        column: x => x.SourceConnectorId,
                        principalTable: "PowerConnectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerConnectorCompatibilities_CompatibleConnectorId",
                table: "PowerConnectorCompatibilities",
                column: "CompatibleConnectorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerConnectorCompatibilities");
        }
    }
}
