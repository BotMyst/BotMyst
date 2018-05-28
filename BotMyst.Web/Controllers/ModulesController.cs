using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BotMyst.Web.Models;
using System.Linq;

namespace BotMyst.Web.Controllers
{
    [Route ("api/[controller]")]
    public class ModulesController : Controller
    {
        private ModulesContext _context;

        public ModulesController (ModulesContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("generateoptions")]
        public async Task<JsonResult> GenerateOptions (ulong guildId)
        {
            if (_context.UtilityOptions.Any (o => o.Id == guildId) == false)
            {
                await _context.AddAsync (new Bot.Options.UtilityOptions { Id = guildId, LmgtfyOptions = new Bot.Options.LmgtfyOptions { Id = guildId} , UserInfoOptions = new Bot.Options.UserInfoOptions { Id = guildId } });
                await _context.SaveChangesAsync ();
            }

            return new JsonResult ($"successfully added {guildId}");
        }
    }
}