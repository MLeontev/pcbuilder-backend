using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedMotherboards2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motherboards_MotherboardFormFactors_MotherboardFormFactorId",
                table: "Motherboards");

            migrationBuilder.DropIndex(
                name: "IX_Motherboards_MotherboardFormFactorId",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "MotherboardFormFactorId",
                table: "Motherboards");

            migrationBuilder.InsertData(
                table: "PcComponents",
                columns: new[] { "Id", "BrandId", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 5, 3, null, "PRO H610M-E DDR4" },
                    { 6, 3, null, "PRO B650-S WIFI" }
                });

            migrationBuilder.InsertData(
                table: "Motherboards",
                columns: new[] { "Id", "FormFactorId", "MaxMemoryCapacity", "MaxMemorySpeed", "MemorySlots", "MemoryTypeId", "MotherboardChipsetId", "SocketId" },
                values: new object[,]
                {
                    { 5, 1, 64, 3200, 2, 2, 1, 1 },
                    { 6, 2, 256, 4800, 4, 3, 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_FormFactorId",
                table: "Motherboards",
                column: "FormFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Motherboards_MotherboardFormFactors_FormFactorId",
                table: "Motherboards",
                column: "FormFactorId",
                principalTable: "MotherboardFormFactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motherboards_MotherboardFormFactors_FormFactorId",
                table: "Motherboards");

            migrationBuilder.DropIndex(
                name: "IX_Motherboards_FormFactorId",
                table: "Motherboards");

            migrationBuilder.DeleteData(
                table: "Motherboards",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Motherboards",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<int>(
                name: "MotherboardFormFactorId",
                table: "Motherboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_MotherboardFormFactorId",
                table: "Motherboards",
                column: "MotherboardFormFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Motherboards_MotherboardFormFactors_MotherboardFormFactorId",
                table: "Motherboards",
                column: "MotherboardFormFactorId",
                principalTable: "MotherboardFormFactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
