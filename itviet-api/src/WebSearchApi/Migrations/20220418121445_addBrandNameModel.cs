using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class addBrandNameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandName",
                columns: table => new
                {
                    BrandNameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    ContractId = table.Column<int>(nullable: false),
                    AgentId = table.Column<int>(nullable: false),
                    LabelId = table.Column<int>(nullable: false),
                    TeamplateId = table.Column<int>(nullable: false),
                    IsTelcoSUB = table.Column<string>(nullable: true),
                    ContractTypeId = table.Column<int>(nullable: false),
                    ApiUser = table.Column<string>(nullable: false),
                    ApiPass = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    ApiLink = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    SendSMS = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    OrganizeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandName", x => x.BrandNameId);
                    table.ForeignKey(
                        name: "FK_BrandName_Organizes_OrganizeId",
                        column: x => x.OrganizeId,
                        principalTable: "Organizes",
                        principalColumn: "OrganizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandName_OrganizeId",
                table: "BrandName",
                column: "OrganizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandName");
        }
    }
}
