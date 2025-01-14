using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cpus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CpuSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sockets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sockets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cpus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    SeriesId = table.Column<int>(type: "integer", nullable: false),
                    SocketId = table.Column<int>(type: "integer", nullable: false),
                    Cores = table.Column<int>(type: "integer", nullable: false),
                    Threads = table.Column<int>(type: "integer", nullable: false),
                    BaseClock = table.Column<decimal>(type: "numeric", nullable: false),
                    BoostClock = table.Column<decimal>(type: "numeric", nullable: false),
                    Tdp = table.Column<int>(type: "integer", nullable: false),
                    IntegratedGpu = table.Column<bool>(type: "boolean", nullable: false),
                    MaxMemoryCapacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cpus_CpuSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "CpuSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cpus_PcComponents_Id",
                        column: x => x.Id,
                        principalTable: "PcComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cpus_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CpuMemories",
                columns: table => new
                {
                    CpuId = table.Column<int>(type: "integer", nullable: false),
                    MemoryTypeId = table.Column<int>(type: "integer", nullable: false),
                    MaxMemorySpeed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuMemories", x => new { x.CpuId, x.MemoryTypeId });
                    table.ForeignKey(
                        name: "FK_CpuMemories_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CpuMemories_MemoryTypes_MemoryTypeId",
                        column: x => x.MemoryTypeId,
                        principalTable: "MemoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuMemories_MemoryTypeId",
                table: "CpuMemories",
                column: "MemoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_SeriesId",
                table: "Cpus",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_SocketId",
                table: "Cpus",
                column: "SocketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CpuMemories");

            migrationBuilder.DropTable(
                name: "Cpus");

            migrationBuilder.DropTable(
                name: "MemoryTypes");

            migrationBuilder.DropTable(
                name: "CpuSeries");

            migrationBuilder.DropTable(
                name: "Sockets");
        }
    }
}
