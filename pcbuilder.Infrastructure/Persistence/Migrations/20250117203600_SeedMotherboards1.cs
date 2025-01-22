using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedMotherboards1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpuSeries_Brands_BrandId",
                table: "CpuSeries");

            migrationBuilder.DropIndex(
                name: "IX_CpuSeries_BrandId",
                table: "CpuSeries");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "CpuSeries");

            migrationBuilder.AddColumn<int>(
                name: "StorageFormFactorId",
                table: "MotherboardStorages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "MSI" });

            migrationBuilder.UpdateData(
                table: "CpuSeries",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Intel Core i5");

            migrationBuilder.UpdateData(
                table: "CpuSeries",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "AMD Ryzen 5");

            migrationBuilder.InsertData(
                table: "MotherboardChipsets",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Intel H610" },
                    { 2, "AMD B650" }
                });

            migrationBuilder.InsertData(
                table: "MotherboardFormFactors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Micro-ATX" },
                    { 2, "ATX" }
                });

            migrationBuilder.InsertData(
                table: "PciSlots",
                columns: new[] { "Id", "Bandwidth", "Version" },
                values: new object[] { 1, 16, "4.0" });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardStorages_StorageFormFactorId",
                table: "MotherboardStorages",
                column: "StorageFormFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MotherboardStorages_StorageFormFactors_StorageFormFactorId",
                table: "MotherboardStorages",
                column: "StorageFormFactorId",
                principalTable: "StorageFormFactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotherboardStorages_StorageFormFactors_StorageFormFactorId",
                table: "MotherboardStorages");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardStorages_StorageFormFactorId",
                table: "MotherboardStorages");

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MotherboardChipsets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MotherboardChipsets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MotherboardFormFactors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MotherboardFormFactors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PciSlots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "StorageFormFactorId",
                table: "MotherboardStorages");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "CpuSeries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CpuSeries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BrandId", "Name" },
                values: new object[] { 1, "Core i5" });

            migrationBuilder.UpdateData(
                table: "CpuSeries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BrandId", "Name" },
                values: new object[] { 2, "Ryzen 5" });

            migrationBuilder.CreateIndex(
                name: "IX_CpuSeries_BrandId",
                table: "CpuSeries",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpuSeries_Brands_BrandId",
                table: "CpuSeries",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
