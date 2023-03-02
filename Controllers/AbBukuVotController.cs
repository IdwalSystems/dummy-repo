using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using MSNK.Models.Operations;
using Rotativa.AspNetCore;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AbBukuVotController : Controller
    {
        public const string modul = "AK003";
        public const string namamodul = "Buku Vot";

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
                carianDari = searchFrom.Trim().ToUpper();
            }

            if (string.IsNullOrEmpty(searchTo))
            {
                carianHingga = "";
            }
            else
            {
                carianHingga = searchTo.Trim().ToUpper();
            }

            ViewData["searchFrom"] = carianDari;
            ViewData["searchTo"] = carianHingga;

            //Ringkasan Debit group by kod Bank AkTerima
            var sql = (from tbl in _context.AbBukuVot.Include(x => x.Vot).Include(x => x.JKW).Include(x=> x.JBahagian)
                       .Where(x => x.Tahun == tahun)
                       .ToList()
                       select new
                       {
                           Id = tbl.VotId,
                           Tahun = tbl.Tahun,
                           JKWId = tbl.JKWId,
                           KW = tbl.JKW.Kod,
                           JBahagianId = tbl.JBahagianId,
                           Bahagian = tbl.JBahagian.Kod,
                           KodAkaun = tbl.Vot.Kod,
                           Perihal = tbl.Vot.Perihal,
                           Debit = tbl.Debit,
                           Kredit = tbl.Kredit,
                           Tanggungan = tbl.Tanggungan,
                           Liabiliti = tbl.Liabiliti,
                           Baki = tbl.Baki

                       }).GroupBy(x => new { x.Tahun,x.KodAkaun, x.KW,x.Bahagian }).ToList();

            IEnumerable<AbBukuVotViewModel> vot = sql.Select(l => new AbBukuVotViewModel
            {
                Id = l.First().Id,
                Tahun = l.Select(x => x.Tahun).FirstOrDefault(),
                KW = l.Select(x => x.KW).FirstOrDefault(),
                JKWId = l.Select(x => x.JKWId).FirstOrDefault(),
                Bahagian = l.Select(x => x.Bahagian).FirstOrDefault(),
                JBahagianId = l.Select(x => x.JBahagianId).FirstOrDefault(),
                KodAkaun = l.Select(x => x.KodAkaun).FirstOrDefault(),
                Perihal = l.Select(x => x.Perihal).FirstOrDefault(),
                Debit = l.Sum(c => c.Debit),
                Kredit = l.Sum(c => c.Kredit),
                Tanggungan = l.Sum(c => c.Tanggungan),
                Liabiliti = l.Sum(c => c.Liabiliti),
                Baki = l.Sum(c => c.Baki)
            }).OrderBy(b => b.KodAkaun).ToList();

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
            else
            {

                return View(new List<AbBukuVotViewModel>());
            }
            //filter range search end

            return View(vot);

        }
        public async Task<IActionResult> Details(
                int? id,
                string tahun,
                int jKWId,
                int jBahagianId,
                string searchFrom,
                string searchTo
                )
        {
            if (id == null)
            {
                return NotFound();
            }

            var abBukuVot = await _context.AbBukuVot
                .Include(x => x.Vot)
                .Include(x => x.JKW)
                .Include(x => x.JBahagian)
                .Where(x => x.Tahun == tahun && x.VotId == id && x.JBahagianId == jBahagianId && x.JKWId == jKWId)
                .FirstOrDefaultAsync();

            ViewData["VotId"] = id;
            ViewData["tahun"] = tahun;
            ViewData["jKWId"] = jKWId;
            ViewData["jBahagianId"] = jBahagianId;

            ViewData["Vot"] = abBukuVot.JKW.Kod + " / " + abBukuVot.JBahagian.Kod + " / " + abBukuVot.Vot.Kod + " - " + abBukuVot.Vot.Perihal;

            var sql = _context.AbBukuVot
                .Include(x => x.Vot).Include(x => x.JKW)
                .Include(x => x.Vot).Include(x => x.JBahagian)
                .Where(x => x.Tahun == tahun && x.VotId == id && x.JBahagianId == jBahagianId && x.JKWId == jKWId)
                .OrderBy(x => x.Tarikh)
                .ToList();

            if (sql == null)
            {
                return NotFound();
            }

            var carianDari = "";
            var carianHingga = "";

            carianDari = searchFrom;
            carianHingga = searchTo;

            ViewData["searchFrom"] = searchFrom;
            ViewData["searchTo"] = searchTo;

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

            return View(sql.OrderBy(b => b.Tarikh));
        }

        // printing List of Carta
        [AllowAnonymous]
        public async Task<IActionResult> PrintBukuVotDetailsPdf(
            int? id,
            string tahun,
            int jKWId,
            int jBahagianId,
            string searchFrom,
            string searchTo)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abBukuVot = await _context.AbBukuVot
                .Include(x => x.Vot)
                .Include(x => x.JKW)
                .Include(x => x.JBahagian)
                .Where(x => x.Tahun == tahun && x.VotId == id && x.JBahagianId == jBahagianId && x.JKWId == jKWId)
                .FirstOrDefaultAsync();

            ViewData["VotId"] = id;
            ViewData["tahun"] = tahun;
            ViewData["jKWId"] = jKWId;
            ViewData["jBahagianId"] = jBahagianId;

            ViewData["Vot"] = abBukuVot.JKW.Kod + " / " + abBukuVot.JBahagian.Kod + " / " + abBukuVot.Vot.Kod + " - " + abBukuVot.Vot.Perihal;

            var sql = _context.AbBukuVot
                .Include(x => x.Vot).Include(x => x.JKW)
                .Include(x => x.Vot).Include(x => x.JBahagian)
                .Where(x => x.Tahun == tahun && x.VotId == id && x.JBahagianId == jBahagianId && x.JKWId == jKWId)
                .OrderBy(x => x.Tarikh)
                .ToList();

            if (sql == null)
            {
                return NotFound();
            }

            var carianDari = "";
            var carianHingga = "";

            carianDari = searchFrom;
            carianHingga = searchTo;

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

            List<AbBukuVotDetailViewModel> list = new List<AbBukuVotDetailViewModel>();

            int bil = 0;
            foreach (var item in sql)
            {

                bil++;

                list.Add(new AbBukuVotDetailViewModel
                {
                    Id = bil,
                    Tarikh = item.Tarikh,
                    Kod = item.Kod,
                    Nama = item.Penerima,
                    NoRujukan = item.Rujukan,
                    Debit = item.Debit,
                    Kredit = item.Kredit,
                    Tanggungan = item.Tanggungan,
                    Liabiliti = item.Liabiliti,
                    Baki = item.Baki
                });
            }
            //filter range search end
            var kw = await _context.JKW.FirstOrDefaultAsync(x => x.Id == jKWId);
            var bahagian = await _context.JBahagian.FirstOrDefaultAsync(x => x.Id == jBahagianId);
            var carta = await _context.AkCarta.FirstOrDefaultAsync(x => x.Id == id);

            var KW = kw.Kod + " - " + kw.Perihal;
            var Bahagian = bahagian.Kod + " - " + bahagian.Perihal;
            var Carta = carta.Kod + " - " + carta.Perihal;

            return new ViewAsPdf("BukuVotDetailsPrintPDF", list.OrderBy(b => b.Tarikh).ToList(),
                new ViewDataDictionary(ViewData) { 
                    {"KW", KW },
                    {"Bahagian", Bahagian },
                    {"Carta", Carta },
                    {"tarDari", carianDari },
                    {"tarHingga", carianHingga } })
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing List of Buku Vot end
    }
}
