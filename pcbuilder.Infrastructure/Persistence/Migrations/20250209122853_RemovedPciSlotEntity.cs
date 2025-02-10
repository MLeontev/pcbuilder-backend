using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPciSlotEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gpus_PciSlots_PciSlotId",
                table: "Gpus");

            migrationBuilder.DropTable(
                name: "MotherboardPciSlots");

            migrationBuilder.DropTable(
                name: "PciSlots");

            migrationBuilder.DropIndex(
                name: "IX_Gpus_PciSlotId",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "PciSlotId",
                table: "Gpus");

            migrationBuilder.AddColumn<int>(
                name: "PcieSlotsCount",
                table: "Motherboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PcieVersion",
                table: "Motherboards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PcieVersion",
                table: "Gpus",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PcieSlotsCount",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "PcieVersion",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "PcieVersion",
                table: "Gpus");

            migrationBuilder.AddColumn<int>(
                name: "PciSlotId",
                table: "Gpus",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PciSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bandwidth = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PciSlots", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_PciSlotId",
                table: "Gpus",
                column: "PciSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardPciSlots_PciSlotId",
                table: "MotherboardPciSlots",
                column: "PciSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gpus_PciSlots_PciSlotId",
                table: "Gpus",
                column: "PciSlotId",
                principalTable: "PciSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
