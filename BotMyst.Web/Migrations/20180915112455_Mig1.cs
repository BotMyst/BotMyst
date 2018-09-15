using Microsoft.EntityFrameworkCore.Migrations;

namespace BotMyst.Web.Migrations
{
    public partial class Mig1 : Migration
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
                name: "CommandDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    CommandOptionsType = table.Column<string>(nullable: true),
                    ModuleDescriptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandDescriptions_Modules_ModuleDescriptionId",
                        column: x => x.ModuleDescriptionId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandDescriptions_ModuleDescriptionId",
                table: "CommandDescriptions",
                column: "ModuleDescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandDescriptions");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
