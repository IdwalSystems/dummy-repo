using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    public class AkTerimaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTerima, int> _akTerimaRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<JNegeri, int> _negeriRepo;
        private readonly AkTerima1IRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private readonly IRepository<AkTerima2, int> _akTerima2Repo;
        private CartTerima _cart;

        public AkTerimaController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<AkTerima, int> akTerimaRepository,
            AkTerima1IRepository<AkTerima1, int> akTerima1Repository,
            IRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int> akBankRepository,
            IRepository<JKW, int> kwRepository,
            IRepository<JNegeri, int> negeriRepository,
            IRepository<AkCarta, int> akCartaRepository,
            CartTerima cart
            )
        {
            _context = context;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akBankRepo = akBankRepository;
            _akTerimaRepo = akTerimaRepository;
            _akTerima1Repo = akTerima1Repository;
            _akTerima2Repo = akTerima2Repository;
            _akCartaRepo = akCartaRepository;
            _cart = cart;
        }

        // GET: AkTerima
        public async Task<IActionResult> Index()
        {
            var akTerima = await _akTerimaRepo.GetAll();
            return View(akTerima);
        }

        // GET: AkTerima/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akTerima.JKWId);
            akTerima.JKW = kw;
            var negeri = await _negeriRepo.GetById(akTerima.JNegeriId);
            akTerima.JNegeri = negeri;
            var akBank = await _akBankRepo.GetById(akTerima.AkBankId);
            akTerima.AkBank = akBank;
            if (akTerima == null)
            {
                return NotFound();
            }

            return View(akTerima);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b=> b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW).OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta = akCartaList;

            List<JCaraBayar> jCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = jCaraBayarList;

        }

        private void PopulateTable(int? id)
        {
            List<AkTerima1> akTerima1Table = _context.AkTerima1
                .Include(b =>b.AkCarta)
                .Where(b=>b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima1 = akTerima1Table;

            List<AkTerima2> akTerima2Table = _context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(b => b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima2 = akTerima2Table;
        }

        private void PopulateCart(AkTerima akTerima)
        {
            List<AkTerima1> akTerima1Table = _context.AkTerima1
                .Include(b => b.AkCarta)
                .Where(b => b.AkTerimaId == akTerima.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTerima1 akTerima1 in akTerima1Table)
            {
                _cart.AddItem1(akTerima1.AkTerimaId,
                               akTerima1.Amaun,
                               akTerima1.AkCartaId);
            }

            List<AkTerima2> akTerima2Table = _context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(b => b.AkTerimaId == akTerima.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach(AkTerima2 akTerima2 in akTerima2Table)
            {
                _cart.AddItem2(akTerima2.AkTerimaId,
                               akTerima2.JCaraBayarId,
                               akTerima2.Amaun,
                               akTerima2.NoCek,
                               akTerima2.JenisCek,
                               akTerima2.KodBankCek,
                               akTerima2.TempatCek,
                               akTerima2.NoSlip,
                               akTerima2.TarSlip);
            }
        }

        // GET: AkTerima/Create
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            return View();
        }

        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkTerima akTerima, int JKWId, int JNegeriId, int AkBankId)
        {
            AkTerima m = new AkTerima();
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15); ;

            if (ModelState.IsValid)
            {
                if (akTerima != null && JNegeriId != 0 && JKWId != 0 && JNegeriId != 0)
                {
                    
                    m.JKWId = JKWId;
                    m.JNegeriId = JNegeriId;
                    m.AkBankId = AkBankId;
                    m.Tahun = akTerima.Tahun;
                    m.NoRujukan = akTerima.NoRujukan;
                    m.Tarikh = akTerima.Tarikh;
                    m.Jumlah = akTerima.Jumlah;
                    m.FlCetak = 0;
                    m.FlPosting = 0;
                    m.FlBatal = 0;
                    m.KodPembayar = akTerima.KodPembayar;
                    m.NoKp = akTerima.NoKp;
                    m.Nama = akTerima.Nama;
                    m.Alamat1 = akTerima.Alamat1;
                    m.Alamat2 = akTerima.Alamat2;
                    m.Alamat3 = akTerima.Alamat3;
                    m.Poskod = akTerima.Poskod;
                    m.Bandar = akTerima.Bandar;
                    m.Tel = akTerima.Tel;
                    m.Emel = akTerima.Emel;
                    m.Sebab = akTerima.Sebab;
                    m.UserId = username;
                    m.TarMasuk = akTerima.TarMasuk;
                    m.TarKemaskini = akTerima.TarKemaskini;

                    m.AkTerima1 = _cart.Lines1.ToArray();
                    m.AkTerima2 = _cart.Lines2.ToArray();

                    await _akTerimaRepo.Insert(m);
                    await _context.SaveChangesAsync();

                    CartEmpty();
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(akTerima);
        }

        // GET: AkTerima/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akTerima.JKWId);
            akTerima.JKW = kw;
            var negeri = await _negeriRepo.GetById(akTerima.JNegeriId);
            akTerima.JNegeri = negeri;
            var akBank = await _akBankRepo.GetById(akTerima.AkBankId);
            akTerima.AkBank = akBank;
            if (akTerima == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akTerima);
            return View(akTerima);
        }

        // POST: AkTerima/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkTerima akTerima, int JKWId, int JNegeriId, int AkBankId)
        {
            if (id != akTerima.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akTerima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTerimaExists(akTerima.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CartEmpty();
                return RedirectToAction(nameof(Index));
            }

            PopulateList();
            PopulateTable(id);
            return View(akTerima);
        }

        // GET: AkTerima/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _context.AkTerima
                .Include(a => a.AkBank)
                .Include(a => a.JKW)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTerima == null)
            {
                return NotFound();
            }

            return View(akTerima);
        }

        // POST: AkTerima/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTerima = await _context.AkTerima.FindAsync(id);
            _context.AkTerima.Remove(akTerima);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTerimaExists(int id)
        {
            return _context.AkTerima.Any(e => e.Id == id);
        }

        public JsonResult GetCaraBayar(JCaraBayar jCaraBayar)
        {
            try
            {
                var result = _context.JCaraBayar.Where(b => b.Id == jCaraBayar.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public JsonResult GetCarta(AkCarta akCarta)
        {
            try
            {
                var result = _context.AkCarta.Where(b => b.Id == akCarta.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public JsonResult CartEmpty()
        {
            try
            {
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null )
                {
                    _cart.AddItem1(akTerima1.AkTerimaId,
                                    akTerima1.Amaun,
                                    akTerima1.AkCartaId);  
                    
                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                if (akTerima2 != null)
                {
                    _cart.AddItem2(
                        akTerima2.AkTerimaId,
                        akTerima2.JCaraBayarId,
                        akTerima2.Amaun,
                        akTerima2.NoCek,
                        akTerima2.JenisCek,
                        akTerima2.KodBankCek,
                        akTerima2.TempatCek,
                        akTerima2.NoSlip,
                        akTerima2.TarSlip);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null)
                {

                    _cart.RemoveItem1(akTerima1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                if (akTerima2 != null)
                {

                    _cart.RemoveItem2(akTerima2.JCaraBayarId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> UpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                AkTerima1 data = await _akTerima1Repo.GetBy2Id(akTerima1.AkTerimaId, akTerima1.AkCartaId);

                return Json(new { result = "OK" , record = data});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null || akTerima1.Amaun != 0)
                {
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akTerima1.AkCartaId);
                    akTerima1.AkCarta = akCarta;
                    await _akTerima1Repo.Insert(akTerima1);
                    await _context.SaveChangesAsync();

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null)
                {
                    var akT1 = await _context.AkTerima1.FirstOrDefaultAsync(x=> x.AkCartaId == akTerima1.AkCartaId && x.AkTerimaId == akTerima1.AkTerimaId);
                    _context.AkTerima1.Remove(akT1);
                    await _context.SaveChangesAsync();

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                _cart.Clear1();

                AkTerima1 akT1 = await _akTerima1Repo.GetById(akTerima1.Id);
                akT1.Amaun = akTerima1.Amaun;
                _context.AkTerima1.Update(akT1);
                await _context.SaveChangesAsync();

                return Json(new { result = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetCart1(AkTerima1 akTerima1)
        {
            try
            {
                AkTerima data = _context.AkTerima.Include(x => x.AkTerima1).ThenInclude(x=>x.AkCarta).FirstOrDefault(x => x.Id == akTerima1.AkTerimaId);

                List<AkTerima1> akT1 = data.AkTerima1.ToList();

                foreach (AkTerima1 item in akT1)
                {
                    _cart.AddItem1(item.AkTerimaId, item.Amaun, item.AkCartaId);
                }

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                List<AkTerima1> akT1 = _context.AkTerima1
                    .Include(x => x.AkCarta)
                    .Where(x => x.AkTerimaId == akTerima1.AkTerimaId)
                    .ToList();

                return Json(new { result = "OK", record = akT1 });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

    }
}
