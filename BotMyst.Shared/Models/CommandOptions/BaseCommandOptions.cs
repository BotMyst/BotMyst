namespace BotMyst.Shared.Models.CommandOptions
{
    public class BaseCommandOptions : CommandOptions
    {
        [Option ("List of roles that can use the command.", OptionTypes.RoleList)]
        public string RoleWhitelist { get; set; }
        [Option ("List of roles that can't use the command.", OptionTypes.RoleList)]
        public string RoleBlacklist { get; set; }
    }
}