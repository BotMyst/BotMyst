using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BotMyst.Web.Models;

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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("getmoduleoptions")]
        public async Task GetModuleOptions (string ModuleType, ulong guildId)
        {

        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("getcommandoptions")]
        public async Task GetCommandOptions (string commandType, ulong guildId)
        {

        }
    }
}