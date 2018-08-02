namespace BotMyst.Web.Models
{
    public class RolePickerModel
    {
        public string GuildId { get; set; }
        public string CommandId { get; set; }
        public string OptionName { get; set; }
        public DiscordRoleModel [] Roles { get; set; }
    }
}