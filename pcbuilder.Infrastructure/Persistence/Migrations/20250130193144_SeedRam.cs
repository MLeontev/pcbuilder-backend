using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedRam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timing",
                table: "Rams");

            migrationBuilder.InsertData(
                table: "PcComponents",
                columns: new[] { "Id", "BrandId", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 7, 4, null, "Vengeance" },
                    { 8, 5, null, "FURY Beast" },
                    { 9, 6, null, "XPG Lancer Blade" }
                });

            migrationBuilder.InsertData(
                table: "Rams",
                columns: new[] { "Id", "Capacity", "Frequency", "MemoryTypeId", "Modules" },
                values: new object[,]
                {
                    { 7, 8, 1900, 1, 2 },
                    { 8, 8, 3200, 2, 2 },
                    { 9, 16, 6000, 3, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rams",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rams",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rams",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PcComponents",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.AddColumn<string>(
                name: "Timing",
                table: "Rams",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
