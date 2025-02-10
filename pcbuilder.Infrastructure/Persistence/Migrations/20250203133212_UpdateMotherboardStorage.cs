using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMotherboardStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardStorages_StorageInterfaces_StorageInterfaceId",
                table: "MotherboardStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardStorages_StorageInterfaceId",
                table: "MotherboardStorages");

            migrationBuilder.DropColumn(
                name: "StorageInterfaceId",
                table: "MotherboardStorages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages",
                columns: new[] { "MotherboardId", "StorageFormFactorId" });

            migrationBuilder.CreateTable(
                name: "MotherboardStorageInterfaces",
                columns: table => new
                {
                    MotherboardStorageId = table.Column<int>(type: "integer", nullable: false),
                    StorageInterfaceId = table.Column<int>(type: "integer", nullable: false),
                    MotherboardStorageSlotMotherboardId = table.Column<int>(type: "integer", nullable: false),
                    MotherboardStorageSlotStorageFormFactorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardStorageInterfaces", x => new { x.MotherboardStorageId, x.StorageInterfaceId });
                    table.ForeignKey(
                        name: "FK_MotherboardStorageInterfaces_MotherboardStorages_Motherboar~",
                        columns: x => new { x.MotherboardStorageSlotMotherboardId, x.MotherboardStorageSlotStorageFormFactorId },
                        principalTable: "MotherboardStorages",
                        principalColumns: new[] { "MotherboardId", "StorageFormFactorId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotherboardStorageInterfaces_StorageInterfaces_StorageInter~",
                        column: x => x.StorageInterfaceId,
                        principalTable: "StorageInterfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorageInterfaces_MotherboardStorageSlotMotherbo~",
                table: "MotherboardStorageInterfaces",
                columns: new[] { "MotherboardStorageSlotMotherboardId", "MotherboardStorageSlotStorageFormFactorId" });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorageInterfaces_StorageInterfaceId",
                table: "MotherboardStorageInterfaces",
                column: "StorageInterfaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherboardStorageInterfaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages");

            migrationBuilder.AddColumn<int>(
                name: "StorageInterfaceId",
                table: "MotherboardStorages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages",
                columns: new[] { "MotherboardId", "StorageInterfaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorages_StorageInterfaceId",
                table: "MotherboardStorages",
                column: "StorageInterfaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardStorages_StorageInterfaces_StorageInterfaceId",
                table: "MotherboardStorages",
                column: "StorageInterfaceId",
                principalTable: "StorageInterfaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
