using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BotMyst.Web.Models;
using System.Collections.Generic;

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
        public async Task GenerateOptions (ulong guildId)
        {
        }
    }
}