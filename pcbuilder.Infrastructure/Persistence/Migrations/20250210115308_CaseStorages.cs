using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CaseStorages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseStorageFormFactors");

            migrationBuilder.AddColumn<int>(
                name: "Slots25",
                table: "Cases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Slots35",
                table: "Cases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slots25",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Slots35",
                table: "Cases");

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

            migrationBuilder.CreateIndex(
                name: "IX_CaseStorageFormFactors_StorageFormFactorId",
                table: "CaseStorageFormFactors",
                column: "StorageFormFactorId");
        }
    }
}
