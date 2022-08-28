using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class add_newField_ScheduleTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "AAATest");

            migrationBuilder.AddColumn<int>(
                name: "OrganizeId",
                table: "ScheduleTemplate",
                nullable: false,
                defaultValue: 0);
      migrationBuilder.Sql("  if EXISTS (Select top 1 OrganizeId From Organizes) begin update ScheduleTemplate set OrganizeId=(Select top 1 OrganizeId From Organizes) end");
      migrationBuilder.CreateIndex(
                name: "IX_ScheduleTemplate_OrganizeId",
                table: "ScheduleTemplate",
                column: "OrganizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleTemplate_Organizes_OrganizeId",
                table: "ScheduleTemplate",
                column: "OrganizeId",
                principalTable: "Organizes",
                principalColumn: "OrganizeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleTemplate_Organizes_OrganizeId",
                table: "ScheduleTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleTemplate_OrganizeId",
                table: "ScheduleTemplate");

            migrationBuilder.DropColumn(
                name: "OrganizeId",
                table: "ScheduleTemplate");

            //migrationBuilder.CreateTable(
            //    name: "AAATest",
            //    columns: table => new
            //    {
            //        ScheduleId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        OrganizeId = table.Column<int>(type: "int", nullable: false),
            //        UpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AAATest", x => x.ScheduleId);
            //    });
        }
    }
}
