using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cpu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "CpuSeries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Intel" },
                    { 2, "AMD" }
                });

            migrationBuilder.InsertData(
                table: "MemoryTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "DDR3" },
                    { 2, "DDR4" },
                    { 3, "DDR5" }
                });

            migrationBuilder.InsertData(
                table: "Sockets",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "LGA1700" },
                    { 2, "AM4" },
                    { 3, "AM5" }
                });

            migrationBuilder.InsertData(
                table: "CpuSeries",
                columns: new[] { "Id", "BrandId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Core i5" },
                    { 2, 2, "Ryzen 5" }
                });

            migrationBuilder.InsertData(
                table: "PcComponents",
                columns: new[] { "Id", "BrandId", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 2, 2, null, "Ryzen 5 5600X 3.7 GHz 6-Core Processor" },
                    { 3, 2, null, "Ryzen 5 7500F 3.7 GHz 6-Core Processor" },
                    { 4, 1, null, "Core i5-12400F 2.5 GHz 6-Core Processor" }
                });

            migrationBuilder.InsertData(
                table: "Cpus",
                columns: new[] { "Id", "BaseClock", "BoostClock", "Cores", "IntegratedGpu", "MaxMemoryCapacity", "SeriesId", "SocketId", "Tdp", "Threads" },
                values: new object[,]
                {
                    { 2, 3.7m, 4.6m, 6, false, 128, 2, 2, 65, 12 },
                    { 3, 3.7m, 5m, 6, false, 128, 2, 3, 65, 12 },
                    { 4, 2.5m, 4.4m, 6, false, 128, 1, 1, 65, 12 }
                });

            migrationBuilder.InsertData(
                table: "CpuMemories",
                columns: new[] { "CpuId", "MemoryTypeId", "MaxMemorySpeed" },
                values: new object[,]
                {
                    { 2, 2, 3200 },
                    { 3, 3, 5200 },
                    { 4, 2, 3200 },
                    { 4, 3, 4800 }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpuSeries_Brands_BrandId",
                table: "CpuSeries");

            migrationBuilder.DropIndex(
                name: "IX_CpuSeries_BrandId",
                table: "CpuSeries");

            migrationBuilder.DeleteData(
                table: "CpuMemories",
                keyColumns: new[] { "CpuId", "MemoryTypeId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "CpuMemories",
                keyColumns: new[] { "CpuId", "MemoryTypeId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "CpuMemories",
                keyColumns: new[] { "CpuId", "MemoryTypeId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "CpuMemories",
                keyColumns: new[] { "CpuId", "MemoryTypeId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "MemoryTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cpus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cpus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cpus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MemoryTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MemoryTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CpuSeries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CpuSeries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sockets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sockets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sockets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "CpuSeries");
        }
    }
}
