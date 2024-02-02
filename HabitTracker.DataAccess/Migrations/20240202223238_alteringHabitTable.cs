using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class alteringHabitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityPerWeek",
                table: "Habits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityPerWeek",
                table: "Habits");
        }
    }
}
