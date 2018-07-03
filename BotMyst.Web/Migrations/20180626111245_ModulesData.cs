using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BotMyst.Web.Migrations
{
    public partial class ModulesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommandDescriptionModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Command = table.Column<string>(nullable: true),
                    CommandOptionsType = table.Column<string>(nullable: true),
                    ModuleDescriptionModelId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandDescriptionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandDescriptionModel_Modules_ModuleDescriptionModelId",
                        column: x => x.ModuleDescriptionModelId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandDescriptionModel_ModuleDescriptionModelId",
                table: "CommandDescriptionModel",
                column: "ModuleDescriptionModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandDescriptionModel");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
