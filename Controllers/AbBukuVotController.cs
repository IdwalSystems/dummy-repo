using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using MSNK.Models.Operations;

namespace MSNK.Controllers
{
    [Authorize(Roles = "Admin , Supervisor")]
    public class AbBukuVotController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;

        public AbBukuVotController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<AbBukuVot, int, string> akBukuVotRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkCarta, int, string> akCartaRepository
            )
        {
            _context = context;
            _userManager = userManager;
            _abBukuVotRepo = akBukuVotRepository;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
        }

        // GET: AbBukuVot
        public async Task<IActionResult> Index(
            string searchYear,
            string searchFrom,
            string searchTo
            )
        {

            var user = await _userManager.GetUserAsync(User);

            var tahun = "";
            if (string.IsNullOrEmpty(searchYear))
            {
                tahun = DateTime.Now.Year.ToString();
            }
            else
            {
                tahun = searchYear;
            }

            ViewData["searchYear"] = tahun;

            var carianDari = "";
            var carianHingga = "";

            if (string.IsNullOrEmpty(searchFrom))
            {
                carianDari = "";
            }
            else
            {
                carianDari = searchFrom;
            }

            if (string.IsNullOrEmpty(searchTo))
            {
                carianHingga = "";
            }
            else
            {
                carianHingga = searchTo;
            }

            ViewData["searchFrom"] = carianDari;
            ViewData["searchTo"] = carianHingga;

            //Ringkasan Debit group by kod Bank AkTerima
            var sql = (from tbl in _context.AbBukuVot.Include(x => x.Vot).Include(x => x.JKW)
                       .Where(x => x.Tahun == tahun)
                       .ToList()
                       select new
                       {
                           Id = tbl.VotId,
                           Tahun = tbl.Tahun,
                           KW = tbl.JKW.Kod,
                           KodAkaun = tbl.Vot.Kod,
                           Perihal = tbl.Vot.Perihal,
                           Debit = tbl.Debit,
                           Kredit = tbl.Kredit,
                           Tanggungan = tbl.Tanggungan,
                           Liabiliti = tbl.Liabiliti,
                           Baki = tbl.Baki

                       }).GroupBy(x => new { x.Tahun, x.Id }).ToList();

            IEnumerable<AbBukuVotViewModel> vot = sql.Select(l => new AbBukuVotViewModel
            {
                Id = l.First().Id,
                Tahun = l.Select(x => x.Tahun).FirstOrDefault(),
                KW = l.Select(x => x.KW).FirstOrDefault(),
                KodAkaun = l.Select(x => x.KodAkaun).FirstOrDefault(),
                Perihal = l.Select(x => x.Perihal).FirstOrDefault(),
                Debit = l.Sum(c => c.Debit),
                Kredit = l.Sum(c => c.Kredit),
                Tanggungan = l.Sum(c => c.Tanggungan),
                Liabiliti = l.Sum(c => c.Liabiliti),
                Baki = l.Sum(c => c.Baki)
            }).ToList();

            //filter range search
            CarianJulat carian = new CarianJulat();

            carian.year = tahun;
            carian.keyword1 = carianDari;
            carian.keyword2 = carianHingga;
            Tuple<string, string> range = Tuple.Create(carian.keyword1, carian.keyword2);

            if (carian.keyword1 != "" && carian.keyword2 != "" )
            {
                vot = vot.Where(s =>
                        range.Item1.CompareTo(s.KodAkaun.Substring(0, range.Item1.Length)) <= 0 &&
                        s.KodAkaun.Substring(0, range.Item2.Length).CompareTo(range.Item2) <= 0)
                        .OrderBy(x => x.KodAkaun).ToList();
            }
            //filter range search end

            return View(vot);

        }
        public async Task<IActionResult> Details(
                int? id,
                string tahun,
                string searchFrom,
                string searchTo
                )
        {
            if (id == null)
            {
                return NotFound();
            }

            var abBukuVot = await _abBukuVotRepo.GetById((int)id);

            ViewData["tahun"] = tahun;
            ViewData["Vot"] = abBukuVot.Vot.Kod + " - " + abBukuVot.Vot.Perihal;

            var sql = _context.AbBukuVot
                .Include(x => x.Vot).Include(x => x.JKW)
                .Where(x=> x.Tahun == tahun && x.VotId == id)
                .OrderBy(x => x.Tarikh)
                .ToList();

            if (sql == null)
            {
                return NotFound();
            }

            var carianDari = "";
            var carianHingga = "";

            if (string.IsNullOrEmpty(searchFrom))
            {
                carianDari = "";
            }
            else
            {
                carianDari = searchFrom;
            }

            if (string.IsNullOrEmpty(searchTo))
            {
                carianHingga = "";
            }
            else
            {
                carianHingga = searchTo;
            }

            ViewData["searchFrom"] = carianDari;
            ViewData["searchTo"] = carianHingga;

            //filter range search
            CarianJulat carian = new CarianJulat();

            carian.year = tahun;
            carian.keyword1 = carianDari;
            carian.keyword2 = carianHingga;

            if (carian.keyword1 != "" && carian.keyword2 != "")
            {
                DateTime date1 = DateTime.Parse(carian.keyword1);
                DateTime date2 = DateTime.Parse(carian.keyword2).AddHours(23.99);

                sql = sql.Where(x => x.Tarikh >= date1
                        && x.Tarikh <= date2).ToList();
            }
            //filter range search end

            return View(sql);
        }
    }
}
