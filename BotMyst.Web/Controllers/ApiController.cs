using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BotMyst.Web.Models;
using BotMyst.Bot.Options.Utility;
using BotMyst.Bot;

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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("getmoduleoptions")]
        public async Task GetModuleOptions (string ModuleType, ulong guildId)
        {

        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route ("getcommandoptions")]
        public async Task GetCommandOptions (string commandOptionsType, ulong guildId)
        {
            foreach (PropertyInfo p in typeof (ModulesContext).GetProperties ())
            {
                if (p.PropertyType.GenericTypeArguments.Length == 1 && p.PropertyType.GenericTypeArguments [0].Name == commandOptionsType)
                {
                    System.Console.WriteLine($"Found the options type: {p.PropertyType.GenericTypeArguments [0].Name}");

                    dynamic dbSet = p.GetValue (_context);
                }
            }
        }
    }
}