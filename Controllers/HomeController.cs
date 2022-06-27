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

                // badge count
                int bilMore5Days = 0;
                int bilLess5Days = 0;
                foreach (var item in akPO)
                {
                    var bakiTarikh = (DateTime.Now - item.Tarikh).Days;
                    if (bakiTarikh > 14)
                    {
                        bilMore5Days++;
                    }
                    else
                    {
                        bilLess5Days++;
                    }
                }
                // badge count end
                // Widget Status PO end

                // Widget Status Nota Minta
                var akNotaMinta = await _context.AkNotaMinta
                    .Include(b => b.AkPembekal)
                    .Where(b => b.FlPosting == 0 )
                    .OrderByDescending(b => b.Tarikh)
                    .ToListAsync();

                // badge count
                int bilKewNM = 0;
                int bilLulusNM = 0;
                foreach (var item in akNotaMinta)
                {
                    if (item.NoSiri == null)
                    {
                        bilKewNM++;
                    } 
                    else
                    {
                        bilLulusNM++;
                    }
                }
                //badge count end
                // Widget Status PO end

                // Widget Status Pendahuluan Pelbagai
                var spPendahuluanPelbagai = await _context.SpPendahuluanPelbagai
                    .Include(b => b.SuPekerja)
                    .Where(b => b.FlPosting == 0)
                    .OrderByDescending(b => b.Tarikh)
                    .ToListAsync();

                // badge count
                int bilKewPP = 0;
                foreach (var item in akNotaMinta)
                {
                    bilKewPP++;
                }
                //badge count end
                // Widget Status Pendahuluan Pelbagai end

                dynamic dyModel = new ExpandoObject();
                dyModel.AkPO = akPO;
                dyModel.bilMore5Days = bilMore5Days;
                dyModel.bilLess5Days = bilLess5Days;
                dyModel.AkNotaMinta = akNotaMinta;
                dyModel.bilKewNM = bilKewNM;
                dyModel.bilLulusNM = bilLulusNM;
                dyModel.SpPendahuluanPelbagai = spPendahuluanPelbagai;
                dyModel.bilKewPP = bilKewPP;
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
