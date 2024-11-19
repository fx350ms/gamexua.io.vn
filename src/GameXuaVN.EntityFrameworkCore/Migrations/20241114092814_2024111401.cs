using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameXuaVN.Migrations
{
    /// <inheritdoc />
    public partial class _2024111401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Groups",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "TotalRate",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "TotalRateCount",
                table: "Files",
                newName: "TotalPlay");

            migrationBuilder.RenameColumn(
                name: "DownloadCount",
                table: "Files",
                newName: "TotalLike");

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "Files",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalDislike",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Page",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "TotalDislike",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "TotalPlay",
                table: "Files",
                newName: "TotalRateCount");

            migrationBuilder.RenameColumn(
                name: "TotalLike",
                table: "Files",
                newName: "DownloadCount");

            migrationBuilder.AddColumn<string>(
                name: "Groups",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TotalRate",
                table: "Files",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
