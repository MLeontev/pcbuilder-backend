using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Coolers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterCoolingSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Size = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterCoolingSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coolers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Tdp = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    WaterCoolingSizeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coolers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coolers_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Coolers_WaterCoolingSizes_WaterCoolingSizeId",
                        column: x => x.WaterCoolingSizeId,
                        principalTable: "WaterCoolingSizes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoolerSockets",
                columns: table => new
                {
                    CoolerId = table.Column<int>(type: "integer", nullable: false),
                    SocketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoolerSockets", x => new { x.CoolerId, x.SocketId });
                    table.ForeignKey(
                        name: "FK_CoolerSockets_Coolers_CoolerId",
                        column: x => x.CoolerId,
                        principalTable: "Coolers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoolerSockets_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coolers_WaterCoolingSizeId",
                table: "Coolers",
                column: "WaterCoolingSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoolerSockets_SocketId",
                table: "CoolerSockets",
                column: "SocketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoolerSockets");

            migrationBuilder.DropTable(
                name: "Coolers");

            migrationBuilder.DropTable(
                name: "WaterCoolingSizes");
        }
    }
}
