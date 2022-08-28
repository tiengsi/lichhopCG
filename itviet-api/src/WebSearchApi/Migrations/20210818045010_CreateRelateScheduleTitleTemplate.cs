using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class CreateRelateScheduleTitleTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleTitleTemplateId",
                table: "Schedule",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ScheduleTitleTemplateId",
                table: "Schedule",
                column: "ScheduleTitleTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_ScheduleTitleTemplate_ScheduleTitleTemplateId",
                table: "Schedule",
                column: "ScheduleTitleTemplateId",
                principalTable: "ScheduleTitleTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_ScheduleTitleTemplate_ScheduleTitleTemplateId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_ScheduleTitleTemplateId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ScheduleTitleTemplateId",
                table: "Schedule");
        }
    }
}
