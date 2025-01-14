using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PowerSupplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PsuEfficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsuEfficiencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    PsuEfficiencyId = table.Column<int>(type: "integer", nullable: false),
                    Power = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerSupplies_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerSupplies_PsuEfficiencies_PsuEfficiencyId",
                        column: x => x.PsuEfficiencyId,
                        principalTable: "PsuEfficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsuPowerConnectors",
                columns: table => new
                {
                    PsuId = table.Column<int>(type: "integer", nullable: false),
                    PowerConnectorId = table.Column<int>(type: "integer", nullable: false),
                    PowerSupplyId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsuPowerConnectors", x => new { x.PsuId, x.PowerConnectorId });
                    table.ForeignKey(
                        name: "FK_PsuPowerConnectors_PowerConnectors_PowerConnectorId",
                        column: x => x.PowerConnectorId,
                        principalTable: "PowerConnectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PsuPowerConnectors_PowerSupplies_PowerSupplyId",
                        column: x => x.PowerSupplyId,
                        principalTable: "PowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerSupplies_PsuEfficiencyId",
                table: "PowerSupplies",
                column: "PsuEfficiencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PsuPowerConnectors_PowerConnectorId",
                table: "PsuPowerConnectors",
                column: "PowerConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_PsuPowerConnectors_PowerSupplyId",
                table: "PsuPowerConnectors",
                column: "PowerSupplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PsuPowerConnectors");

            migrationBuilder.DropTable(
                name: "PowerSupplies");

            migrationBuilder.DropTable(
                name: "PsuEfficiencies");
        }
    }
}
