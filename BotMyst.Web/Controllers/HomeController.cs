using Microsoft.AspNetCore.Mvc;

namespace BotMyst.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index () =>
            View ();
    }
}