using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BotMyst.Bot;
using BotMyst.Web.Models;
using BotMyst.Bot.Options.Utility;
using BotMyst.Web.Helpers;

namespace BotMyst.Web.Controllers
{
    [Route ("/api")]
    public class ApiController : Controller
    {
        private ModulesContext modulesContext;
        private ModuleOptionsContext moduleOptionsContext;

        public ApiController (ModulesContext modulesContext, ModuleOptionsContext moduleOptionsContext)
        {
            this.modulesContext = modulesContext;
            this.moduleOptionsContext = moduleOptionsContext;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("generateoptions")]
        public async Task GenerateOptions (ulong guildId)
        {
            if (moduleOptionsContext.LmgtfyOptions.Any (a => a.GuildId == guildId) == false)
                moduleOptionsContext.LmgtfyOptions.Add (new LmgtfyOptions { GuildId = guildId });

            if (moduleOptionsContext.UserInfoOptions.Any (a => a.GuildId == guildId) == false)
                moduleOptionsContext.UserInfoOptions.Add (new UserInfoOptions { GuildId = guildId });

            await moduleOptionsContext.SaveChangesAsync ();
        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("getcommandoptions")]
        public IActionResult GetCommandOptions (string commandOptionsType, ulong guildId) =>
            Ok (ApiHelpers.GetCommandOptions (moduleOptionsContext, commandOptionsType, guildId));

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("sendmoduledata")]
        public async Task SendModuleData ([FromBody] Models.ModuleDescriptionModel[] modules)
        {
            // TODO: Don't remove it each time as the ID changes
            foreach (var s in modulesContext.Modules)
                modulesContext.Remove (s);

            foreach (var m in modules)
            {
                await modulesContext.AddAsync (m);
            }

            await modulesContext.SaveChangesAsync ();
        }
    }
}