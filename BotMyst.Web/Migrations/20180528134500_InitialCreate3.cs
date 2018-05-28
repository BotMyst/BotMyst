using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BotMyst.Web.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfoOptions",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false),
                    DeleteInvocationMessage = table.Column<bool>(nullable: false),
                    Dm = table.Column<bool>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    ExcludeChannels = table.Column<string>(nullable: true),
                    ExcludeRoles = table.Column<string>(nullable: true),
                    IncludeChannels = table.Column<string>(nullable: true),
                    IncludeRoles = table.Column<string>(nullable: true),
                    ShowCreatedAt = table.Column<bool>(nullable: false),
                    ShowGame = table.Column<bool>(nullable: false),
                    ShowJoinedAt = table.Column<bool>(nullable: false),
                    ShowOnlineStatus = table.Column<bool>(nullable: false),
                    ShowRoles = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfoOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UtilityOptions",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false),
                    LmgtfyOptionsId = table.Column<ulong>(nullable: true),
                    UserInfoOptionsId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilityOptions_LmgtfyOptions_LmgtfyOptionsId",
                        column: x => x.LmgtfyOptionsId,
                        principalTable: "LmgtfyOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UtilityOptions_UserInfoOptions_UserInfoOptionsId",
                        column: x => x.UserInfoOptionsId,
                        principalTable: "UserInfoOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtilityOptions_LmgtfyOptionsId",
                table: "UtilityOptions",
                column: "LmgtfyOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilityOptions_UserInfoOptionsId",
                table: "UtilityOptions",
                column: "UserInfoOptionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilityOptions");

            migrationBuilder.DropTable(
                name: "UserInfoOptions");
        }
    }
}
