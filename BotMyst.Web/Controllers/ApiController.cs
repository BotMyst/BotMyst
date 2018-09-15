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
using BotMyst.Web.Helpers;
using BotMyst.Bot.Options.Utility;

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
            // (#58) TODO: Make it so this isn't so ugly...
            if (moduleOptionsContext.LmgtfyOptions.Any (a => a.GuildId == guildId) == false)
                moduleOptionsContext.LmgtfyOptions.Add (new LmgtfyOptions { GuildId = guildId });

            if (moduleOptionsContext.UserInfoOptions.Any (a => a.GuildId == guildId) == false)
                moduleOptionsContext.UserInfoOptions.Add (new UserInfoOptions { GuildId = guildId });

            if (moduleOptionsContext.AvatarOptions.Any (a => a.GuildId == guildId) == false)
                moduleOptionsContext.AvatarOptions.Add (new AvatarOptions { GuildId = guildId });

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
            foreach (var module in modules)
            {
                if (modulesContext.Modules.Any (m => m.Name == module.Name))
                {
                    var dbModule = await modulesContext.Modules.FirstAsync (m => m.Name == module.Name);

                    var query = from c in modulesContext.CommandDescriptions
                                where (c.ModuleDescriptionId == dbModule.Id)
                                select c;

                    foreach (var command in module.CommandDescriptions)
                    {
                        if (query.Any (c => c.Name == command.Name))
                        {
                            var dbCommand = await query.FirstAsync (c => c.Name == command.Name);
                            command.Id = dbCommand.Id;
                            dbCommand = command;
                        }
                        else
                        {
                            await modulesContext.AddAsync (command);
                        }
                    }
                }
                else
                {
                    await modulesContext.AddAsync (module);
                }
            }

            await modulesContext.SaveChangesAsync ();
        }
    }
}