using Microsoft.EntityFrameworkCore.Migrations;

namespace BotMyst.Web.Migrations.CommandOptions
{
    public partial class LmgtfyDefaultSearchEngine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultSearchEngine",
                table: "LmgtfyOptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultSearchEngine",
                table: "LmgtfyOptions");
        }
    }
}
