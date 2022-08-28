using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class add_PermissionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionsMasterData",
                columns: table => new
                {
                    NamePermission = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    PermissionsMasterDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamePermissionParent = table.Column<string>(nullable: false),
                    PermissionLevel = table.Column<string>(nullable: true),
                    RouteTemple = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsMasterData", x => x.NamePermission);
                });

            migrationBuilder.CreateTable(
                name: "PermissionMasterAndRoleMapping",
                columns: table => new
                {
                    PermissionMasterAndRoleMappingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    NamePermission = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: false),
                    IsAllow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionMasterAndRoleMapping", x => x.PermissionMasterAndRoleMappingId);
                    table.ForeignKey(
                        name: "FK_PermissionMasterAndRoleMapping_PermissionsMasterData_NamePermission",
                        column: x => x.NamePermission,
                        principalTable: "PermissionsMasterData",
                        principalColumn: "NamePermission",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionMasterAndRoleMapping_NamePermission",
                table: "PermissionMasterAndRoleMapping",
                column: "NamePermission");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionMasterAndRoleMapping");

            migrationBuilder.DropTable(
                name: "PermissionsMasterData");
        }
    }
}
