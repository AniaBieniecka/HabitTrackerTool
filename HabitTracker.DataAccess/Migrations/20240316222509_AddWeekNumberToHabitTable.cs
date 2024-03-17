using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWeekNumberToHabitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekNumber",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekNumber",
                table: "Habits");
        }
    }
}
