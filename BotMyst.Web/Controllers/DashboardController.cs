using Microsoft.AspNetCore.Mvc;

namespace BotMyst.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index ()
        {
            if (User.Identity.IsAuthenticated == false)
                return RedirectToAction ("Login", "Account");
                
            return View ();
        }
    }
}