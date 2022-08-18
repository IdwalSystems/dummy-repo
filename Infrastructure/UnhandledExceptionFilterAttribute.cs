using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSNK.Data;
using MSNK.Models.Administration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace MSNK.Infrastructure
{
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<UnhandledExceptionFilterAttribute> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UnhandledExceptionFilterAttribute(
            ILogger<UnhandledExceptionFilterAttribute> logger, 
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public override void OnException(ExceptionContext context)
        {
            ExceptionLogger logger = new ExceptionLogger();

            logger.LogTime = DateTime.Now;
            logger.UserName = context.HttpContext.User.Identity.Name;
            logger.TraceIdentifier = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            logger.ControllerName = context.ActionDescriptor.DisplayName;
            logger.ExceptionMessage = context.Exception.Message;
            logger.ExceptionStackTrace = context.Exception.StackTrace;
            logger.ExceptionType = context.Exception.GetType().FullName;
            logger.Source = context.Exception.Source;
            logger.UrlRequest = context.HttpContext.Request.Path.ToString();

            _db.Add(logger);
            _db.SaveChanges();

        }
    }
}
