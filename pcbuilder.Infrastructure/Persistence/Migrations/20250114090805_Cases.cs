using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MaxGpuLength = table.Column<int>(type: "integer", nullable: false),
                    MaxCoolerHeight = table.Column<int>(type: "integer", nullable: false),
                    MaxPsuLength = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardFormFactors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardFormFactors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseStorageFormFactors",
                columns: table => new
                {
                    CaseId = table.Column<int>(type: "integer", nullable: false),
                    StorageFormFactorId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStorageFormFactors", x => new { x.CaseId, x.StorageFormFactorId });
                    table.ForeignKey(
                        name: "FK_CaseStorageFormFactors_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseStorageFormFactors_StorageFormFactors_StorageFormFactor~",
                        column: x => x.StorageFormFactorId,
                        principalTable: "StorageFormFactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseWaterCoolingSizes",
                columns: table => new
                {
                    CaseId = table.Column<int>(type: "integer", nullable: false),
                    WaterCoolingSizeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseWaterCoolingSizes", x => new { x.CaseId, x.WaterCoolingSizeId });
                    table.ForeignKey(
                        name: "FK_CaseWaterCoolingSizes_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseWaterCoolingSizes_WaterCoolingSizes_WaterCoolingSizeId",
                        column: x => x.WaterCoolingSizeId,
                        principalTable: "WaterCoolingSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseMotherboardFormFactors",
                columns: table => new
                {
                    CaseId = table.Column<int>(type: "integer", nullable: false),
                    MotherboardFormFactorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseMotherboardFormFactors", x => new { x.CaseId, x.MotherboardFormFactorId });
                    table.ForeignKey(
                        name: "FK_CaseMotherboardFormFactors_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseMotherboardFormFactors_MotherboardFormFactors_Motherboa~",
                        column: x => x.MotherboardFormFactorId,
                        principalTable: "MotherboardFormFactors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseMotherboardFormFactors_MotherboardFormFactorId",
                table: "CaseMotherboardFormFactors",
                column: "MotherboardFormFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseStorageFormFactors_StorageFormFactorId",
                table: "CaseStorageFormFactors",
                column: "StorageFormFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseWaterCoolingSizes_WaterCoolingSizeId",
                table: "CaseWaterCoolingSizes",
                column: "WaterCoolingSizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseMotherboardFormFactors");

            migrationBuilder.DropTable(
                name: "CaseStorageFormFactors");

            migrationBuilder.DropTable(
                name: "CaseWaterCoolingSizes");

            migrationBuilder.DropTable(
                name: "MotherboardFormFactors");

            migrationBuilder.DropTable(
                name: "Cases");
        }
    }
}
