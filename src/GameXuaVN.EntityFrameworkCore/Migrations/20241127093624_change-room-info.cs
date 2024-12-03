using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameXuaVN.Migrations
{
    /// <inheritdoc />
    public partial class changeroominfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayerName",
                table: "RoomParticipants",
                newName: "UserName");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "RoomParticipants",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "RoomParticipants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RoomParticipants");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "RoomParticipants",
                newName: "PlayerName");

            migrationBuilder.AlterColumn<long>(
                name: "RoomId",
                table: "RoomParticipants",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
