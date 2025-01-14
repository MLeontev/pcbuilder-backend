using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pcbuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PcComponentImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PcComponents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PcComponents");
        }
    }
}
