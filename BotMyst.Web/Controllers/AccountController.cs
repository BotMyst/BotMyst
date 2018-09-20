using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BotMyst.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login () =>
            Challenge (new AuthenticationProperties () { RedirectUri = "/Dashboard" });
    }
}