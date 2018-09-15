using Microsoft.EntityFrameworkCore.Migrations;

namespace BotMyst.Web.Migrations.ModuleOptions
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LmgtfyOptions",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    Dm = table.Column<bool>(nullable: false),
                    DeleteInvocationMessage = table.Column<bool>(nullable: false),
                    RoleWhitelist = table.Column<string>(nullable: true),
                    RoleBlacklist = table.Column<string>(nullable: true),
                    ChannelWhitelist = table.Column<string>(nullable: true),
                    ChannelBlacklist = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LmgtfyOptions", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "UserInfoOptions",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Enabled = table.Column<bool>(nullable: false),
                    Dm = table.Column<bool>(nullable: false),
                    DeleteInvocationMessage = table.Column<bool>(nullable: false),
                    RoleWhitelist = table.Column<string>(nullable: true),
                    RoleBlacklist = table.Column<string>(nullable: true),
                    ChannelWhitelist = table.Column<string>(nullable: true),
                    ChannelBlacklist = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfoOptions", x => x.GuildId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LmgtfyOptions");

            migrationBuilder.DropTable(
                name: "UserInfoOptions");
        }
    }
}
