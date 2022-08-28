using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class new_field_organizeId_ScheduleTitleTemplate_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "ScheduleTitleTemplate",
                nullable: false,
                defaultValue: 0);
      migrationBuilder.Sql("if EXISTS (Select top 1 OrganizeId From Organizes) begin update ScheduleTitleTemplate set OrganizeId=(Select top 1 OrganizeId From Organizes) end");

      migrationBuilder.CreateIndex(
                name: "IX_ScheduleTitleTemplate_OrganizeId",
                table: "ScheduleTitleTemplate",
                column: "OrganizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleTitleTemplate_Organizes_OrganizeId",
                table: "ScheduleTitleTemplate",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleTitleTemplate_Organizes_OrganizeId",
                table: "ScheduleTitleTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleTitleTemplate_OrganizeId",
                table: "ScheduleTitleTemplate");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "ScheduleTitleTemplate");
        }
    }
}
