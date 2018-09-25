using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BotMyst.Web.Controllers
{
    [Route ("api")]
    public class ApiController : Controller
    {
        /// <summary>
        /// You can send a GET request to this endpoint to check if the API works.
        /// </summary>
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("confirmapi")]
        public IActionResult ConfirmApi ()
        {
            return Ok ("BotMyst API works!");
        }
    }
}