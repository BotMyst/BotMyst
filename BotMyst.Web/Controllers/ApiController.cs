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
        private ModulesContext _context;

        public ApiController (ModulesContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("generateoptions")]
        public async Task GenerateOptions (ulong guildId)
        {
            if (_context.LmgtfyOptions.Any (a => a.GuildId == guildId) == false)
                _context.LmgtfyOptions.Add (new LmgtfyOptions { GuildId = guildId });

            if (_context.UserInfoOptions.Any (a => a.GuildId == guildId) == false)
                _context.UserInfoOptions.Add (new UserInfoOptions { GuildId = guildId });

            await _context.SaveChangesAsync ();
        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("getcommandoptions")]
        public JsonResult GetCommandOptions (string commandOptionsType, ulong guildId)
        {
            CommandOptions result = null;

            foreach (PropertyInfo p in typeof (ModulesContext).GetProperties ())
            {
                if (p.PropertyType.GenericTypeArguments.Length == 1 && p.PropertyType.GenericTypeArguments [0].Name == commandOptionsType)
                {
                    System.Console.WriteLine($"Found the options type: {p.PropertyType.GenericTypeArguments [0].Name}");

                    dynamic dbSet = p.GetValue (_context);

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
    }
}