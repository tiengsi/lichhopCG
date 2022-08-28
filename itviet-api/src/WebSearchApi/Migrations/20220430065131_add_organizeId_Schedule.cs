using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class add_organizeId_Schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);
      migrationBuilder.Sql("  if EXISTS (Select top 1 OrganizeId From Organizes) begin update Schedule set OrganizeId=(Select top 1 OrganizeId From Organizes) end");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_OrganizeId",
                table: "Schedule",
                column: "OrganizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Organizes_OrganizeId",
                table: "Schedule",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Organizes_OrganizeId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_OrganizeId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "Schedule");
        }
    }
}
