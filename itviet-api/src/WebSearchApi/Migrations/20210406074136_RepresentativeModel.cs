using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class RepresentativeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Representative",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representative", x => new { x.DepartmentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Representative_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Representative_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleHistory",
                columns: table => new
                {
                    ScheduleHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(nullable: false),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    ScheduleTime = table.Column<string>(nullable: true),
                    ScheduleContent = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    OtherLocation = table.Column<string>(nullable: true),
                    OtherHost = table.Column<string>(nullable: true),
                    Participants = table.Column<string>(nullable: true),
                    ParticipantDisplay = table.Column<string>(nullable: true),
                    VersionNo = table.Column<int>(nullable: false),
                    IsSendEmail = table.Column<bool>(nullable: false),
                    ISendSMS = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleHistory", x => x.ScheduleHistoryId);
                    table.ForeignKey(
                        name: "FK_ScheduleHistory_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleHistory_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Representative_UserId",
                table: "Representative",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleHistory_Id",
                table: "ScheduleHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleHistory_LocationId",
                table: "ScheduleHistory",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Representative");

            migrationBuilder.DropTable(
                name: "ScheduleHistory");
        }
    }
}
