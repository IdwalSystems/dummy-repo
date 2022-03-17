using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MSNK.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using MSNK.Models.Administration;
using Microsoft.AspNetCore.Identity;
using MSNK.Models.Modules.IRepository;
using System.Dynamic;
using System.Threading.Tasks;
using MSNK.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSNK.Models.Modules;

namespace MSNK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int, string> _poRepo;

        public HomeController(
            ApplicationDbContext context,
            ILogger<HomeController> logger,  
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int, string> poRepo)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _poRepo = poRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            else
            {
                // Widget Status PO
                var akPO = await _context.AkPO
                    .Include(b => b.AkPembekal)
                    .Where(b=> b.FlPosting == 0)
                    .OrderByDescending(b=> b.Tarikh)
                    .ToListAsync();
                // filtering day balance
                var BenchDate = DateTime.Today.AddDays(-5);
                akPO = akPO.Where(b => b.Tarikh < BenchDate).ToList();
                // Widget Status PO end

                dynamic dyModel = new ExpandoObject();
                dyModel.AkPO = akPO;
                return View(dyModel);
            }
            
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            //test
            return View();
        }

        public IActionResult UnderMaintainance()
        {
            //test
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
