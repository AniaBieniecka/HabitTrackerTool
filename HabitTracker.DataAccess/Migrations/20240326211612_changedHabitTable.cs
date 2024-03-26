using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changedHabitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWeeklyCommitmentFulfilled",
                table: "Habits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWeeklyCommitmentFulfilled",
                table: "Habits");
        }
    }
}
