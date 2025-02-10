using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CaseMaxMotherboardFormFactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseMotherboardFormFactors");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "MotherboardFormFactors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxMotherboardFormFactorId",
                table: "Cases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_MaxMotherboardFormFactorId",
                table: "Cases",
                column: "MaxMotherboardFormFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_MotherboardFormFactors_MaxMotherboardFormFactorId",
                table: "Cases",
                column: "MaxMotherboardFormFactorId",
                principalTable: "MotherboardFormFactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_MotherboardFormFactors_MaxMotherboardFormFactorId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_MaxMotherboardFormFactorId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "MotherboardFormFactors");

            migrationBuilder.DropColumn(
                name: "MaxMotherboardFormFactorId",
                table: "Cases");

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
        }
    }
}
