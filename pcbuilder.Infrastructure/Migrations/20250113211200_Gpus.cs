using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Gpus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GpuChipsets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpuChipsets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PciSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version = table.Column<string>(type: "text", nullable: false),
                    Bandwidth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PciSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerConnectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Pins = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerConnectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gpus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MemoryCapacity = table.Column<int>(type: "integer", nullable: false),
                    BusWidth = table.Column<int>(type: "integer", nullable: false),
                    CoreClock = table.Column<int>(type: "integer", nullable: false),
                    BoostClock = table.Column<int>(type: "integer", nullable: false),
                    PciSlotId = table.Column<int>(type: "integer", nullable: false),
                    Tdp = table.Column<int>(type: "integer", nullable: false),
                    Length = table.Column<int>(type: "integer", nullable: false),
                    ChipsetId = table.Column<int>(type: "integer", nullable: false),
                    RecommendedPsuPower = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gpus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gpus_GpuChipsets_ChipsetId",
                        column: x => x.ChipsetId,
                        principalTable: "GpuChipsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gpus_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gpus_PciSlots_PciSlotId",
                        column: x => x.PciSlotId,
                        principalTable: "PciSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GpuPowerConnectors",
                columns: table => new
                {
                    GpuId = table.Column<int>(type: "integer", nullable: false),
                    PowerConnectorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpuPowerConnectors", x => new { x.PowerConnectorId, x.GpuId });
                    table.ForeignKey(
                        name: "FK_GpuPowerConnectors_Gpus_GpuId",
                        column: x => x.GpuId,
                        principalTable: "Gpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GpuPowerConnectors_PowerConnectors_PowerConnectorId",
                        column: x => x.PowerConnectorId,
                        principalTable: "PowerConnectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GpuPowerConnectors_GpuId",
                table: "GpuPowerConnectors",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ChipsetId",
                table: "Gpus",
                column: "ChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_PciSlotId",
                table: "Gpus",
                column: "PciSlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GpuPowerConnectors");

            migrationBuilder.DropTable(
                name: "Gpus");

            migrationBuilder.DropTable(
                name: "PowerConnectors");

            migrationBuilder.DropTable(
                name: "GpuChipsets");

            migrationBuilder.DropTable(
                name: "PciSlots");
        }
    }
}
