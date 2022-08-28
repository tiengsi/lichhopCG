using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class UpdateEmailLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromEmail",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "ToEmail",
                table: "EmailLogs");

            migrationBuilder.AddColumn<int>(
                name: "OtherParticipantId",
                table: "EmailLogs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendEmailIsSuccess",
                table: "EmailLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SendSmsIsSuccess",
                table: "EmailLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_OtherParticipantId",
                table: "EmailLogs",
                column: "OtherParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_OtherParticipant_OtherParticipantId",
                table: "EmailLogs",
                column: "OtherParticipantId",
                principalTable: "OtherParticipant",
                principalColumn: "OtherParticipantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_OtherParticipant_OtherParticipantId",
                table: "EmailLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_OtherParticipantId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "OtherParticipantId",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "SendEmailIsSuccess",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "SendSmsIsSuccess",
                table: "EmailLogs");

            migrationBuilder.AddColumn<string>(
                name: "FromEmail",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "EmailLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ToEmail",
                table: "EmailLogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
