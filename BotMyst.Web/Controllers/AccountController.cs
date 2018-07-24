using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BotMyst.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login ()
        {
            return Challenge (new AuthenticationProperties { RedirectUri = "/Dashboard/Index" } );
        }

        public IActionResult Logout ()
        {
            Response.Cookies.Delete ("DiscordCookie");
            return RedirectToAction ("Index", "Home");
        }
    }
}