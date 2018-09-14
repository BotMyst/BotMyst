namespace BotMyst.Web.Models
{
    public interface IDiscordNameable : IDiscordObject
    {
        string Name { get; set; }
    }
}