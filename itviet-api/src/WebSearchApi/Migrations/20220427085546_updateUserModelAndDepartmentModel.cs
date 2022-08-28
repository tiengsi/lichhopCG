using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class updateUserModelAndDepartmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "Department",
                nullable: false,
                defaultValue: "");   
      migrationBuilder.Sql("  if EXISTS (Select top 1 OrganizeId From Organizes) begin update Department set OrganizeId=(Select top 1 OrganizeId From Organizes) end");

      migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_OrganizeId",
                table: "Department",
                column: "OrganizeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizeId",
                table: "AspNetUsers",
                column: "OrganizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organizes_OrganizeId",
                table: "AspNetUsers",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Organizes_OrganizeId",
                table: "Department",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organizes_OrganizeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Organizes_OrganizeId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_OrganizeId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrganizeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "AspNetUsers");
        }
    }
}
