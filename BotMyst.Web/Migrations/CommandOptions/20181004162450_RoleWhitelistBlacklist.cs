using Microsoft.EntityFrameworkCore.Migrations;

namespace BotMyst.Web.Migrations.CommandOptions
{
    public partial class RoleWhitelistBlacklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvatarOptions",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    RoleWhitelist = table.Column<string>(nullable: true),
                    RoleBlacklist = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarOptions", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "ClearOptions",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    RoleWhitelist = table.Column<string>(nullable: true),
                    RoleBlacklist = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClearOptions", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "LmgtfyOptions",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    RoleWhitelist = table.Column<string>(nullable: true),
                    RoleBlacklist = table.Column<string>(nullable: true),
                    DefaultSearchEngine = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LmgtfyOptions", x => x.GuildId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvatarOptions");

            migrationBuilder.DropTable(
                name: "ClearOptions");

            migrationBuilder.DropTable(
                name: "LmgtfyOptions");
        }
    }
}
