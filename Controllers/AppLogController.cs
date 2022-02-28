using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AppLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppLog
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppLog.ToListAsync());
        }
   
    }
}
