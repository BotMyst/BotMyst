using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace BotMyst.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login ()
            => Challenge (new AuthenticationProperties { RedirectUri = "/Dashboard/Index" } );

        public IActionResult Logout ()
        {
            Response.Cookies.Delete ("DiscordCookie");
            return RedirectToAction ("Index", "Home");
        }
    }
}