using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class init_NewField_ScheduleTableAndJobSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandNameId",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoSendAtScheduledTime",
                table: "Schedule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MeetingLink",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSchedule",
                table: "JobSchedule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleTime",
                table: "JobSchedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandNameId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "IsAutoSendAtScheduledTime",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "MeetingLink",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "IsSchedule",
                table: "JobSchedule");

            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                table: "JobSchedule");
        }
    }
}
