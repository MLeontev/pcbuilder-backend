using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pcbuilder.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PsuFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PsuPowerConnectors",
                table: "PsuPowerConnectors");

            migrationBuilder.DropIndex(
                name: "IX_PsuPowerConnectors_PowerSupplyId",
                table: "PsuPowerConnectors");

            migrationBuilder.DropColumn(
                name: "PsuId",
                table: "PsuPowerConnectors");

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "PowerSupplies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PsuPowerConnectors",
                table: "PsuPowerConnectors",
                columns: new[] { "PowerSupplyId", "PowerConnectorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PsuPowerConnectors",
                table: "PsuPowerConnectors");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "PowerSupplies");

            migrationBuilder.AddColumn<int>(
                name: "PsuId",
                table: "PsuPowerConnectors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PsuPowerConnectors",
                table: "PsuPowerConnectors",
                columns: new[] { "PsuId", "PowerConnectorId" });

            migrationBuilder.CreateIndex(
                name: "IX_PsuPowerConnectors_PowerSupplyId",
                table: "PsuPowerConnectors",
                column: "PowerSupplyId");
        }
    }
}
