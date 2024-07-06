using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUserIdToViewSettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ViewSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ViewSettings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ViewSettings_UserId",
                table: "ViewSettings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ViewSettings_AspNetUsers_UserId",
                table: "ViewSettings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViewSettings_AspNetUsers_UserId",
                table: "ViewSettings");

            migrationBuilder.DropIndex(
                name: "IX_ViewSettings_UserId",
                table: "ViewSettings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ViewSettings");

            migrationBuilder.InsertData(
                table: "Scores",
                columns: new[] { "Id", "LevelId", "ScoreValue" },
                values: new object[] { 1, 0, 0 });

            migrationBuilder.InsertData(
                table: "ViewSettings",
                columns: new[] { "Id", "Color", "IconDone", "IconPartiallyDone" },
                values: new object[] { 1, "00CED1", "bi bi-check-square-fill", "bi bi-check-square" });
        }
    }
}
