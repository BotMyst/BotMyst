using Microsoft.EntityFrameworkCore.Migrations;

namespace BotMyst.Web.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleDescriptions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleDescriptions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CommandDescriptions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Command = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    ModuleDescriptionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandDescriptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CommandDescriptions_ModuleDescriptions_ModuleDescriptionID",
                        column: x => x.ModuleDescriptionID,
                        principalTable: "ModuleDescriptions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandDescriptions_ModuleDescriptionID",
                table: "CommandDescriptions",
                column: "ModuleDescriptionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandDescriptions");

            migrationBuilder.DropTable(
                name: "ModuleDescriptions");
        }
    }
}
