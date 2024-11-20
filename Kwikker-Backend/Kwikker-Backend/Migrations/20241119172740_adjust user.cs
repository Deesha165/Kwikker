using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kwikker_Backend.Migrations
{
    /// <inheritdoc />
    public partial class adjustuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPicture",
                table: "AspNetUsers");
        }
    }
}
