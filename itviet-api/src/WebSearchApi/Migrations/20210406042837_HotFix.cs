using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class HotFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrMSchedule");

            migrationBuilder.DropTable(
                name: "GrMUser");

            migrationBuilder.DropTable(
                name: "GroupMeeting");

            migrationBuilder.DropColumn(
                name: "IsPartsFromGM",
                table: "Schedule");

            migrationBuilder.AddColumn<bool>(
                name: "ISendSMS",
                table: "Schedule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSendEmail",
                table: "Schedule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantDisplay",
                table: "Schedule",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISendSMS",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "IsSendEmail",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ParticipantDisplay",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsPartsFromGM",
                table: "Schedule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GroupMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMeeting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrMSchedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    GrMeetingId = table.Column<int>(type: "int", nullable: false)
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
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GrMeetingId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_GrMSchedule_GrMeetingId",
                table: "GrMSchedule",
                column: "GrMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_GrMUser_GrMeetingId",
                table: "GrMUser",
                column: "GrMeetingId");
        }
    }
}
