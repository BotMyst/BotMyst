namespace BotMyst.Shared.Models.CommandOptions
{
    public class BaseCommandOptions : CommandOptions
    {
        public string RoleWhitelist { get; set; }
        public string RoleBlacklist { get; set; }
    }
}