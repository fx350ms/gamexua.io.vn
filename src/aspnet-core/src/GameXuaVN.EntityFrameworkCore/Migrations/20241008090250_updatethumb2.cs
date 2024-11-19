using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameXuaVN.Migrations
{
    /// <inheritdoc />
    public partial class updatethumb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Images_ThumbnailId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ThumbnailId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbnail",
                table: "Files",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ThumbnailId",
                table: "Files",
                column: "ThumbnailId",
                unique: true,
                filter: "[ThumbnailId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Images_ThumbnailId",
                table: "Files",
                column: "ThumbnailId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
