using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    public class AkTerimaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkTerima, int> _akTerimaRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;
        private readonly IRepository<KW, int> _kwRepo;
        private readonly IRepository<Negeri, int> _negeriRepo;
        private readonly IRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkTerima2, int> _akTerima2Repo;

        public AkTerimaController(
            ApplicationDbContext context,
            IRepository<AkTerima, int> akTerimaRepository,
            IRepository<AkTerima1, int> akTerima1Repository,
            IRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int> akBankRepository,
            IRepository<KW, int> kwRepository,
            IRepository<Negeri, int> negeriRepository
            )
        {
            _context = context;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akBankRepo = akBankRepository;
            _akTerimaRepo = akTerimaRepository;
            _akTerima1Repo = akTerima1Repository;
            _akTerima2Repo = akTerima2Repository;
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
            var kw = await _kwRepo.GetById(akTerima.KWId);
            akTerima.KW = kw;
            var negeri = await _negeriRepo.GetById(akTerima.NegeriId);
            akTerima.Negeri = negeri;
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
            List<KW> kwList = _context.KW.OrderBy(b => b.Kod).ToList();
            ViewBag.Kw = kwList;

            List<Negeri> negeriList = _context.Negeri.OrderBy(b => b.Kod).ToList();
            ViewBag.Negeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b=> b.Bank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

        }
        // GET: AkTerima/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkTerima akTerima, int KWId, int NegeriId, int AkBankId, int AkTerima1Id, int AkTerima2Id)
        {
            AkTerima m = new AkTerima();
            AkTerima1 t1 = new AkTerima1();
            AkTerima2 t2 = new AkTerima2();

            if (ModelState.IsValid)
            {
                if (akTerima != null && NegeriId != 0 && KWId != 0 && NegeriId != 0 && AkTerima1Id != 0 && AkTerima2Id != 0)
                {

                    m.KWId = KWId;
                    m.NegeriId = NegeriId;
                    m.AkBankId = AkBankId;
                    m.NoRujukan = akTerima.NoRujukan;
                    m.Tarikh = akTerima.Tarikh;
                    m.Jumlah = akTerima.Jumlah;
                    m.FlCetak = 0;
                    m.FlPosting = 0;
                    m.FlBatal = 0;
                    m.KodPembayar = akTerima.KodPembayar;
                    await _akTerimaRepo.Insert(m);
                    await _akTerimaRepo.Save();

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

            var akTerima = await _context.AkTerima.FindAsync(id);
            if (akTerima == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTerima.AkBankId);
            ViewData["KWId"] = new SelectList(_context.KW, "Id", "Kod", akTerima.KWId);
            ViewData["NegeriId"] = new SelectList(_context.Negeri, "Id", "Kod", akTerima.NegeriId);
            return View(akTerima);
        }

        // POST: AkTerima/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tahun,KWId,NoRujukan,Tarikh,Jumlah,AkBankId,FlCetak,FlPosting,FlBatal,KodPembayar,NoKp,Nama,Alamat1,Alamat2,Alamat3,Poskod,Bandar,NegeriId,Tel,Emel,Sebab,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTerima akTerima)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTerima.AkBankId);
            ViewData["KWId"] = new SelectList(_context.KW, "Id", "Kod", akTerima.KWId);
            ViewData["NegeriId"] = new SelectList(_context.Negeri, "Id", "Kod", akTerima.NegeriId);
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
                .Include(a => a.KW)
                .Include(a => a.Negeri)
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
    }
}
