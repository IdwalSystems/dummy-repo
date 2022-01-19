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
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles = "Admin , Supervisor")]
    public class AbBukuVotController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;

        public AbBukuVotController(
            ApplicationDbContext context,
            IRepository<AbBukuVot, int, string> akBukuVotRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkCarta, int, string> akCartaRepository
            )
        {
            _context = context;
            _abBukuVotRepo = akBukuVotRepository;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
        }

        // GET: AbBukuVot
        public async Task<IActionResult> Index()
        {
            var vot = await _abBukuVotRepo.GetAll();
            return View(vot);
        }
    }
}
