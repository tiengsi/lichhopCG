using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class GroupDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupParticipant",
                columns: table => new
                {
                    GroupParticipantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    GroupParticipantName = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupParticipant", x => x.GroupParticipantId);
                });

            migrationBuilder.CreateTable(
                name: "GroupDepartment",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false),
                    GroupParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDepartment", x => new { x.DepartmentId, x.GroupParticipantId });
                    table.ForeignKey(
                        name: "FK_GroupDepartment_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDepartment_GroupParticipant_GroupParticipantId",
                        column: x => x.GroupParticipantId,
                        principalTable: "GroupParticipant",
                        principalColumn: "GroupParticipantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupDepartment_GroupParticipantId",
                table: "GroupDepartment",
                column: "GroupParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupDepartment");

            migrationBuilder.DropTable(
                name: "GroupParticipant");
        }
    }
}
