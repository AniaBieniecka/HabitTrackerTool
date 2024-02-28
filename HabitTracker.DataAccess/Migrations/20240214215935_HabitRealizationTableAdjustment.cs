using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HabitRealizationTableAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRealizations_TimePeriods_DaysRangeId",
                table: "HabitRealizations");

            migrationBuilder.RenameColumn(
                name: "DaysRangeId",
                table: "HabitRealizations",
                newName: "HabitId");

            migrationBuilder.RenameIndex(
                name: "IX_HabitRealizations_DaysRangeId",
                table: "HabitRealizations",
                newName: "IX_HabitRealizations_HabitId");

            migrationBuilder.AlterColumn<int>(
                name: "IfExecuted",
                table: "HabitRealizations",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "HabitRealizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRealizations_Habits_HabitId",
                table: "HabitRealizations",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRealizations_Habits_HabitId",
                table: "HabitRealizations");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "HabitRealizations");

            migrationBuilder.RenameColumn(
                name: "HabitId",
                table: "HabitRealizations",
                newName: "DaysRangeId");

            migrationBuilder.RenameIndex(
                name: "IX_HabitRealizations_HabitId",
                table: "HabitRealizations",
                newName: "IX_HabitRealizations_DaysRangeId");

            migrationBuilder.AlterColumn<bool>(
                name: "IfExecuted",
                table: "HabitRealizations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRealizations_TimePeriods_DaysRangeId",
                table: "HabitRealizations",
                column: "DaysRangeId",
                principalTable: "TimePeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
