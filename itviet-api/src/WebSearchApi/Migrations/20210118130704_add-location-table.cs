using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class addlocationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchedulePosition",
                table: "Schedule");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OtherLocation",
                table: "Schedule",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_LocationId",
                table: "Schedule",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Location_LocationId",
                table: "Schedule",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Location_LocationId",
                table: "Schedule");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_LocationId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "OtherLocation",
                table: "Schedule");

            migrationBuilder.AddColumn<string>(
                name: "SchedulePosition",
                table: "Schedule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
