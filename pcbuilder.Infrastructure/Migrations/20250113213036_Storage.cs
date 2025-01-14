using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Storage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorageFormFactors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageFormFactors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageInterfaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageInterfaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    ReadSpeed = table.Column<int>(type: "integer", nullable: false),
                    WriteSpeed = table.Column<int>(type: "integer", nullable: false),
                    StorageTypeId = table.Column<int>(type: "integer", nullable: false),
                    StorageInterfaceId = table.Column<int>(type: "integer", nullable: false),
                    StorageFormFactorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storage_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storage_StorageFormFactors_StorageFormFactorId",
                        column: x => x.StorageFormFactorId,
                        principalTable: "StorageFormFactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storage_StorageInterfaces_StorageInterfaceId",
                        column: x => x.StorageInterfaceId,
                        principalTable: "StorageInterfaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storage_StorageTypes_StorageTypeId",
                        column: x => x.StorageTypeId,
                        principalTable: "StorageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Storage_StorageFormFactorId",
                table: "Storage",
                column: "StorageFormFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_StorageInterfaceId",
                table: "Storage",
                column: "StorageInterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_StorageTypeId",
                table: "Storage",
                column: "StorageTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "StorageFormFactors");

            migrationBuilder.DropTable(
                name: "StorageInterfaces");

            migrationBuilder.DropTable(
                name: "StorageTypes");
        }
    }
}
