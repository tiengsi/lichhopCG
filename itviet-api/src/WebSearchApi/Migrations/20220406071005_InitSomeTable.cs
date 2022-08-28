using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class InitSomeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizes",
                columns: table => new
                {
                    OrganizeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OrganizeParentId = table.Column<int>(nullable: false),
                    CodeName = table.Column<string>(maxLength: 256, nullable: true),
                    OtherName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 40, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizes", x => x.OrganizeId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalNotes",
                columns: table => new
                {
                    PersonalNotesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ContentNote = table.Column<string>(type: "ntext", nullable: true),
                    ScheduleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalNotes", x => x.PersonalNotesId);
                    table.ForeignKey(
                        name: "FK_PersonalNotes_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalNotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalSchedules",
                columns: table => new
                {
                    PersonalScheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Fromdate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalSchedules", x => x.PersonalScheduleId);
                    table.ForeignKey(
                        name: "FK_PersonalSchedules_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledAttendances",
                columns: table => new
                {
                    ScheduledAttendanceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    OtherParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledAttendances", x => x.ScheduledAttendanceId);
                    table.ForeignKey(
                        name: "FK_ScheduledAttendances_OtherParticipant_OtherParticipantId",
                        column: x => x.OtherParticipantId,
                        principalTable: "OtherParticipant",
                        principalColumn: "OtherParticipantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledAttendances_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledResultDocuments",
                columns: table => new
                {
                    ScheduledResultDocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    DocumentUpdatedDate = table.Column<DateTime>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledResultDocuments", x => x.ScheduledResultDocumentId);
                    table.ForeignKey(
                        name: "FK_ScheduledResultDocuments_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledResultReports",
                columns: table => new
                {
                    ScheduledResultReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    ReportContent = table.Column<string>(nullable: true),
                    ReportTime = table.Column<DateTime>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledResultReports", x => x.ScheduledResultReportId);
                    table.ForeignKey(
                        name: "FK_ScheduledResultReports_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledResultReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTypes",
                columns: table => new
                {
                    ScheduleTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    OrganizeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTypes", x => x.ScheduleTypeId);
                    table.ForeignKey(
                        name: "FK_ScheduleTypes_Organizes_OrganizeId",
                        column: x => x.OrganizeId,
                        principalTable: "Organizes",
                        principalColumn: "OrganizeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalNotes_ScheduleId",
                table: "PersonalNotes",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalNotes_UserId",
                table: "PersonalNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalSchedules_UserId",
                table: "PersonalSchedules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAttendances_OtherParticipantId",
                table: "ScheduledAttendances",
                column: "OtherParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAttendances_UserId",
                table: "ScheduledAttendances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledResultDocuments_ScheduleId",
                table: "ScheduledResultDocuments",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledResultReports_ScheduleId",
                table: "ScheduledResultReports",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledResultReports_UserId",
                table: "ScheduledResultReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTypes_OrganizeId",
                table: "ScheduleTypes",
                column: "OrganizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalNotes");

            migrationBuilder.DropTable(
                name: "PersonalSchedules");

            migrationBuilder.DropTable(
                name: "ScheduledAttendances");

            migrationBuilder.DropTable(
                name: "ScheduledResultDocuments");

            migrationBuilder.DropTable(
                name: "ScheduledResultReports");

            migrationBuilder.DropTable(
                name: "ScheduleTypes");

            migrationBuilder.DropTable(
                name: "Organizes");
        }
    }
}
