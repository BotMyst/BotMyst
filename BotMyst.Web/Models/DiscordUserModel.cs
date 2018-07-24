using System.Security.Claims;

namespace BotMyst.Web.Models
{
    public class DiscordUserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public bool Bot { get; set; }
        public bool MFAEnable { get; set; }
        public bool Verified { get; set; }
        public string Email { get; set; }
    }
}