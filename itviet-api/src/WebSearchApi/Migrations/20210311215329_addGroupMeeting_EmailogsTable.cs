using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class addGroupMeeting_EmailogsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherHost",
                table: "Schedule",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromEmail = table.Column<string>(nullable: true),
                    ToEmail = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    SendDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    ScheduleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLogs_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMeeting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrMSchedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(nullable: false),
                    GrMeetingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrMSchedule", x => new { x.ScheduleId, x.GrMeetingId });
                    table.ForeignKey(
                        name: "FK_GrMSchedule_GroupMeeting_GrMeetingId",
                        column: x => x.GrMeetingId,
                        principalTable: "GroupMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrMSchedule_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrMUser",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    GrMeetingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrMUser", x => new { x.UserId, x.GrMeetingId });
                    table.ForeignKey(
                        name: "FK_GrMUser_GroupMeeting_GrMeetingId",
                        column: x => x.GrMeetingId,
                        principalTable: "GroupMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrMUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_ScheduleId",
                table: "EmailLogs",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_UserId",
                table: "EmailLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GrMSchedule_GrMeetingId",
                table: "GrMSchedule",
                column: "GrMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_GrMUser_GrMeetingId",
                table: "GrMUser",
                column: "GrMeetingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "GrMSchedule");

            migrationBuilder.DropTable(
                name: "GrMUser");

            migrationBuilder.DropTable(
                name: "GroupMeeting");

            migrationBuilder.DropColumn(
                name: "OtherHost",
                table: "Schedule");
        }
    }
}
