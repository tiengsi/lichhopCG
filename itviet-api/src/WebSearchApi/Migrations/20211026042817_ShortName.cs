using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class ShortName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Department",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "AspNetUsers");
        }
    }
}
