using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Motherboards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotherboardChipsets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardChipsets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motherboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MotherboardChipsetId = table.Column<int>(type: "integer", nullable: false),
                    SocketId = table.Column<int>(type: "integer", nullable: false),
                    FormFactorId = table.Column<int>(type: "integer", nullable: false),
                    MotherboardFormFactorId = table.Column<int>(type: "integer", nullable: false),
                    MemoryTypeId = table.Column<int>(type: "integer", nullable: false),
                    MemorySlots = table.Column<int>(type: "integer", nullable: false),
                    MaxMemoryCapacity = table.Column<int>(type: "integer", nullable: false),
                    MaxMemorySpeed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motherboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motherboards_MemoryTypes_MemoryTypeId",
                        column: x => x.MemoryTypeId,
                        principalTable: "MemoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Motherboards_MotherboardChipsets_MotherboardChipsetId",
                        column: x => x.MotherboardChipsetId,
                        principalTable: "MotherboardChipsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Motherboards_MotherboardFormFactors_MotherboardFormFactorId",
                        column: x => x.MotherboardFormFactorId,
                        principalTable: "MotherboardFormFactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Motherboards_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Motherboards_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardPciSlots",
                columns: table => new
                {
                    MotherboardId = table.Column<int>(type: "integer", nullable: false),
                    PciSlotId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardPciSlots", x => new { x.MotherboardId, x.PciSlotId });
                    table.ForeignKey(
                        name: "FK_MotherboardPciSlots_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotherboardPciSlots_PciSlots_PciSlotId",
                        column: x => x.PciSlotId,
                        principalTable: "PciSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardPowerConnectors",
                columns: table => new
                {
                    MotherboardId = table.Column<int>(type: "integer", nullable: false),
                    PowerConnectorId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardPowerConnectors", x => new { x.MotherboardId, x.PowerConnectorId });
                    table.ForeignKey(
                        name: "FK_MotherboardPowerConnectors_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotherboardPowerConnectors_PowerConnectors_PowerConnectorId",
                        column: x => x.PowerConnectorId,
                        principalTable: "PowerConnectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardStorages",
                columns: table => new
                {
                    MotherboardId = table.Column<int>(type: "integer", nullable: false),
                    StorageInterfaceId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardStorages", x => new { x.MotherboardId, x.StorageInterfaceId });
                    table.ForeignKey(
                        name: "FK_MotherboardStorages_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotherboardStorages_StorageInterfaces_StorageInterfaceId",
                        column: x => x.StorageInterfaceId,
                        principalTable: "StorageInterfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardPciSlots_PciSlotId",
                table: "MotherboardPciSlots",
                column: "PciSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardPowerConnectors_PowerConnectorId",
                table: "MotherboardPowerConnectors",
                column: "PowerConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_MemoryTypeId",
                table: "Motherboards",
                column: "MemoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_MotherboardChipsetId",
                table: "Motherboards",
                column: "MotherboardChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_MotherboardFormFactorId",
                table: "Motherboards",
                column: "MotherboardFormFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_SocketId",
                table: "Motherboards",
                column: "SocketId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorages_StorageInterfaceId",
                table: "MotherboardStorages",
                column: "StorageInterfaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherboardPciSlots");

            migrationBuilder.DropTable(
                name: "MotherboardPowerConnectors");

            migrationBuilder.DropTable(
                name: "MotherboardStorages");

            migrationBuilder.DropTable(
                name: "Motherboards");

            migrationBuilder.DropTable(
                name: "MotherboardChipsets");
        }
    }
}
