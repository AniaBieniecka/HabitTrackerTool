using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changedNamesInHabitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityPerWeek",
                table: "Habits",
                newName: "WeeklyGoal");

            migrationBuilder.RenameColumn(
                name: "IsWeeklyCommitmentFulfilled",
                table: "Habits",
                newName: "IsWeeklyGoalAchieved");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeeklyGoal",
                table: "Habits",
                newName: "QuantityPerWeek");

            migrationBuilder.RenameColumn(
                name: "IsWeeklyGoalAchieved",
                table: "Habits",
                newName: "IsWeeklyCommitmentFulfilled");
        }
    }
}
