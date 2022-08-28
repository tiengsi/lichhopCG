using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class updateLocationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "Location",
                nullable: false,
                defaultValue:"");
      migrationBuilder.Sql("if EXISTS (Select top 1 OrganizeId From Organizes) begin update Location set OrganizeId=(Select top 1 OrganizeId From Organizes) end");

      migrationBuilder.CreateIndex(
                name: "IX_Location_OrganizeId",
                table: "Location",
                column: "OrganizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Organizes_OrganizeId",
                table: "Location",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Organizes_OrganizeId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_OrganizeId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "Location");
        }
    }
}
