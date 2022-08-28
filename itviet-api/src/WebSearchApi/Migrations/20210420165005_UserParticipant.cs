using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class UserParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserParticipant",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    GroupParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserParticipant", x => new { x.UserId, x.GroupParticipantId });
                    table.ForeignKey(
                        name: "FK_UserParticipant_GroupParticipant_GroupParticipantId",
                        column: x => x.GroupParticipantId,
                        principalTable: "GroupParticipant",
                        principalColumn: "GroupParticipantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserParticipant_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserParticipant_GroupParticipantId",
                table: "UserParticipant",
                column: "GroupParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserParticipant");
        }
    }
}
