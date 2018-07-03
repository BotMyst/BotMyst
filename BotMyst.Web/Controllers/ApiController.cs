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
        public JsonResult GetCommandOptions (string commandOptionsType, ulong guildId)
        {
            CommandOptions result = null;

            foreach (PropertyInfo p in typeof (ModuleOptionsContext).GetProperties ())
            {
                if (p.PropertyType.GenericTypeArguments.Length == 1 && p.PropertyType.GenericTypeArguments [0].Name == commandOptionsType)
                {
                    System.Console.WriteLine($"Found the options type: {p.PropertyType.GenericTypeArguments [0].Name}");

                    dynamic dbSet = p.GetValue (moduleOptionsContext);

                    List<CommandOptions> commandOptions = new List<CommandOptions> ();
                    foreach (var s in dbSet)
                    {
                        commandOptions.Add (s);
                    }

                    result = commandOptions.Find (c => c.GuildId == guildId);
                    return new JsonResult (result);
                }
            }

            return new JsonResult (result);
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("sendmoduledata")]
        public async Task SendModuleData ([FromBody] Models.ModuleDescriptionModel[] modules)
        {
            foreach (var s in modulesContext.Modules)
                modulesContext.Remove (s);

            foreach (var m in modules)
            {
                await modulesContext.AddAsync (m);
            }

            await modulesContext.SaveChangesAsync ();

            foreach (var s in modulesContext.Modules)
            {
                System.Console.WriteLine(s.Name);

                foreach (var c in s.CommandDescriptions)
                    System.Console.WriteLine(c.Name);
            }
        }
    }
}