using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class habitWeekModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRealizations_Habits_HabitId",
                table: "HabitRealizations");

            migrationBuilder.DropColumn(
                name: "IsWeeklyGoalAchieved",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "WeekNumber",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "WeeklyGoal",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Habits");

            migrationBuilder.RenameColumn(
                name: "HabitId",
                table: "HabitRealizations",
                newName: "HabitWeekId");

            migrationBuilder.RenameIndex(
                name: "IX_HabitRealizations_HabitId",
                table: "HabitRealizations",
                newName: "IX_HabitRealizations_HabitWeekId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Habits",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "HabitWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    WeeklyGoal = table.Column<int>(type: "int", nullable: false),
                    IsWeeklyGoalAchieved = table.Column<bool>(type: "bit", nullable: false),
                    HabitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitWeeks_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitWeeks_HabitId",
                table: "HabitWeeks",
                column: "HabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRealizations_HabitWeeks_HabitWeekId",
                table: "HabitRealizations",
                column: "HabitWeekId",
                principalTable: "HabitWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRealizations_HabitWeeks_HabitWeekId",
                table: "HabitRealizations");

            migrationBuilder.DropTable(
                name: "HabitWeeks");

            migrationBuilder.RenameColumn(
                name: "HabitWeekId",
                table: "HabitRealizations",
                newName: "HabitId");

            migrationBuilder.RenameIndex(
                name: "IX_HabitRealizations_HabitWeekId",
                table: "HabitRealizations",
                newName: "IX_HabitRealizations_HabitId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Habits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AddColumn<bool>(
                name: "IsWeeklyGoalAchieved",
                table: "Habits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WeekNumber",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyGoal",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRealizations_Habits_HabitId",
                table: "HabitRealizations",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
