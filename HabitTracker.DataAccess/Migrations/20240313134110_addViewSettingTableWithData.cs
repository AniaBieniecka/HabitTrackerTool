using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addViewSettingTableWithData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViewSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconPartiallyDone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconDone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ViewSettings",
                columns: new[] { "Id", "Color", "IconDone", "IconPartiallyDone" },
                values: new object[] { 1, "00CED1", "bi bi-check-square-fill", "bi bi-check-square" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewSettings");
        }
    }
}
