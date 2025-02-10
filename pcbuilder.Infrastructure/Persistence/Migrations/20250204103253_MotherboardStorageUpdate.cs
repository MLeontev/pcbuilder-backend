using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MotherboardStorageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardStorageInterfaces_MotherboardStorages_Motherboar~",
                table: "MotherboardStorageInterfaces");

            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardStorages_StorageFormFactors_StorageFormFactorId",
                table: "MotherboardStorages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardStorages_StorageFormFactorId",
                table: "MotherboardStorages");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardStorageInterfaces_MotherboardStorageSlotMotherbo~",
                table: "MotherboardStorageInterfaces");

            migrationBuilder.DropColumn(
                name: "MotherboardStorageSlotMotherboardId",
                table: "MotherboardStorageInterfaces");

            migrationBuilder.DropColumn(
                name: "MotherboardStorageSlotStorageFormFactorId",
                table: "MotherboardStorageInterfaces");

            migrationBuilder.RenameColumn(
                name: "StorageFormFactorId",
                table: "MotherboardStorages",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MotherboardStorages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MotherboardStorageFormFactors",
                columns: table => new
                {
                    MotherboardStorageId = table.Column<int>(type: "integer", nullable: false),
                    StorageFormFactorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardStorageFormFactors", x => new { x.MotherboardStorageId, x.StorageFormFactorId });
                    table.ForeignKey(
                        name: "FK_MotherboardStorageFormFactors_MotherboardStorages_Motherboa~",
                        column: x => x.MotherboardStorageId,
                        principalTable: "MotherboardStorages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotherboardStorageFormFactors_StorageFormFactors_StorageFor~",
                        column: x => x.StorageFormFactorId,
                        principalTable: "StorageFormFactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorages_MotherboardId",
                table: "MotherboardStorages",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorageFormFactors_StorageFormFactorId",
                table: "MotherboardStorageFormFactors",
                column: "StorageFormFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardStorageInterfaces_MotherboardStorages_Motherboar~",
                table: "MotherboardStorageInterfaces",
                column: "MotherboardStorageId",
                principalTable: "MotherboardStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardStorageInterfaces_MotherboardStorages_Motherboar~",
                table: "MotherboardStorageInterfaces");

            migrationBuilder.DropTable(
                name: "MotherboardStorageFormFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardStorages_MotherboardId",
                table: "MotherboardStorages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MotherboardStorages",
                newName: "StorageFormFactorId");

            migrationBuilder.AlterColumn<int>(
                name: "StorageFormFactorId",
                table: "MotherboardStorages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "MotherboardStorageSlotMotherboardId",
                table: "MotherboardStorageInterfaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MotherboardStorageSlotStorageFormFactorId",
                table: "MotherboardStorageInterfaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotherboardStorages",
                table: "MotherboardStorages",
                columns: new[] { "MotherboardId", "StorageFormFactorId" });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorages_StorageFormFactorId",
                table: "MotherboardStorages",
                column: "StorageFormFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorageInterfaces_MotherboardStorageSlotMotherbo~",
                table: "MotherboardStorageInterfaces",
                columns: new[] { "MotherboardStorageSlotMotherboardId", "MotherboardStorageSlotStorageFormFactorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardStorageInterfaces_MotherboardStorages_Motherboar~",
                table: "MotherboardStorageInterfaces",
                columns: new[] { "MotherboardStorageSlotMotherboardId", "MotherboardStorageSlotStorageFormFactorId" },
                principalTable: "MotherboardStorages",
                principalColumns: new[] { "MotherboardId", "StorageFormFactorId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardStorages_StorageFormFactors_StorageFormFactorId",
                table: "MotherboardStorages",
                column: "StorageFormFactorId",
                principalTable: "StorageFormFactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
