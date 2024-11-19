using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameXuaVN.Migrations
{
    /// <inheritdoc />
    public partial class gamegroupsname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Groups",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Groups",
                table: "Files");
        }
    }
}
