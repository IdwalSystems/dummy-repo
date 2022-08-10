using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MSNK.Models.Modules.ViewModel;
using System.Collections.Generic;
using MSNK.Models.Modules;

namespace MSNK.Controllers
{
    [Authorize]
    public class AbBelanjawanSemasaController : Controller
    {
        public const string modul = "BJ002";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CustomIRepository<string, int> _customRepo;

        public AbBelanjawanSemasaController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            CustomIRepository<string, int> customRepo)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(
            int JKWId,
            int JBahagianId, 
            string tahun, 
            DateTime tarHingga)
        {
            List<AbBelanjawanSemasaViewModel> vm = new List<AbBelanjawanSemasaViewModel>();

            List<AbWaran> waran = await _customRepo.GetAbWaranBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);
            return View(vm);
        }
    }
}
