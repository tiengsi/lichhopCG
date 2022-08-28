using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class add_organizeId_GroupParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "GroupParticipant",
                nullable: false,
              defaultValue: "");
      migrationBuilder.Sql("if EXISTS (Select top 1 OrganizeId From Organizes) begin update GroupParticipant set OrganizeId=(Select top 1 OrganizeId From Organizes) end");

      migrationBuilder.CreateIndex(
                name: "IX_GroupParticipant_OrganizeId",
                table: "GroupParticipant",
                column: "OrganizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupParticipant_Organizes_OrganizeId",
                table: "GroupParticipant",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupParticipant_Organizes_OrganizeId",
                table: "GroupParticipant");

            migrationBuilder.DropIndex(
                name: "IX_GroupParticipant_OrganizeId",
                table: "GroupParticipant");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "GroupParticipant");
        }
    }
}
