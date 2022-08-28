using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class UpdateGroupParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherParticipant_Schedule_ScheduleId",
                table: "OtherParticipant");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "OtherParticipant",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GroupParticipantId",
                table: "OtherParticipant",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherParticipant_GroupParticipantId",
                table: "OtherParticipant",
                column: "GroupParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherParticipant_GroupParticipant_GroupParticipantId",
                table: "OtherParticipant",
                column: "GroupParticipantId",
                principalTable: "GroupParticipant",
                principalColumn: "GroupParticipantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherParticipant_Schedule_ScheduleId",
                table: "OtherParticipant",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherParticipant_GroupParticipant_GroupParticipantId",
                table: "OtherParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherParticipant_Schedule_ScheduleId",
                table: "OtherParticipant");

            migrationBuilder.DropIndex(
                name: "IX_OtherParticipant_GroupParticipantId",
                table: "OtherParticipant");

            migrationBuilder.DropColumn(
                name: "GroupParticipantId",
                table: "OtherParticipant");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "OtherParticipant",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherParticipant_Schedule_ScheduleId",
                table: "OtherParticipant",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
