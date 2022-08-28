using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class AddFields_ScheduleEndDate_ScheduleEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleEndDate",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleEndTime",
                table: "Schedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleEndDate",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ScheduleEndTime",
                table: "Schedule");
        }
    }
}
