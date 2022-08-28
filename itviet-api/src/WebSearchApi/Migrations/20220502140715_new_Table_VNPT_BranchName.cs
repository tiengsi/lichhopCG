using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class new_Table_VNPT_BranchName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandName");

            migrationBuilder.CreateTable(
                name: "VNPT_BrandName",
                columns: table => new
                {
                    BrandNameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ContractType = table.Column<int>(nullable: false),
                    ApiUserName = table.Column<string>(nullable: false),
                    ApiPassword = table.Column<string>(nullable: false),
                    BrandName = table.Column<string>(nullable: false),
                    ApiLink = table.Column<string>(nullable: false),
                    OrganizeId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VNPT_BrandName", x => x.BrandNameId);
                    table.ForeignKey(
                        name: "FK_VNPT_BrandName_Organizes_OrganizeId",
                        column: x => x.OrganizeId,
                        principalTable: "Organizes",
                        principalColumn: "OrganizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VNPT_BrandName_OrganizeId",
                table: "VNPT_BrandName",
                column: "OrganizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VNPT_BrandName");

            migrationBuilder.CreateTable(
                name: "BrandName",
                columns: table => new
                {
                    BrandNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    ApiLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTelcoSUB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelId = table.Column<int>(type: "int", nullable: false),
                    OrganizeId = table.Column<int>(type: "int", nullable: false),
                    SendSMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamplateId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}
