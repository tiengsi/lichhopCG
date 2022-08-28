using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class new_Table_Viettel_BranchName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Viettel_BrandName",
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
                    CPCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viettel_BrandName", x => x.BrandNameId);
                    table.ForeignKey(
                        name: "FK_Viettel_BrandName_Organizes_OrganizeId",
                        column: x => x.OrganizeId,
                        principalTable: "Organizes",
                        principalColumn: "OrganizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Viettel_BrandName_OrganizeId",
                table: "Viettel_BrandName",
                column: "OrganizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Viettel_BrandName");
        }
    }
}
