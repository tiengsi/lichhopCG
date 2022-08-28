using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class OtherParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Participants");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Participants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                columns: new[] { "DepartmentId", "ScheduleId" });

            migrationBuilder.CreateTable(
                name: "OtherParticipant",
                columns: table => new
                {
                    OtherParticipantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ScheduleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherParticipant", x => x.OtherParticipantId);
                    table.ForeignKey(
                        name: "FK_OtherParticipant_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherParticipant_ScheduleId",
                table: "OtherParticipant",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Department_DepartmentId",
                table: "Participants",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Department_DepartmentId",
                table: "Participants");

            migrationBuilder.DropTable(
                name: "OtherParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Participants");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                columns: new[] { "UserId", "ScheduleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
