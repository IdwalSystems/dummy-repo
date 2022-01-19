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
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize]
    public class AkTunaiCVController : Controller
    {

        public const string modul = "TR002";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTunaiCV, int, string> _akTunaiCVRepo;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartTunaiCV _cart;

        public AkTunaiCVController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTunaiCV, int, string> akTunaiCVRepository,
            IRepository<SuPekerja, int, string> suPekerjaRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
             CartTunaiCV cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akTunaiCVRepo = akTunaiCVRepository;
            _suPekerjaRepo = suPekerjaRepository;
            _kwRepo = kwRepository;
            _akPembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _cart = cart;
        }

        // GET: AkTunaiCV
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkTunaiCV.Include(a => a.AkPembekal).Include(a => a.AkTunaiRuncit).Include(a => a.SuPekerja);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkTunaiCV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _context.AkTunaiCV
                .Include(a => a.AkPembekal)
                .Include(a => a.AkTunaiRuncit)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTunaiCV == null)
            {
                return NotFound();
            }

            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Create
        public IActionResult Create()
        {
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank");
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id");
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id");
            return View();
        }

        // POST: AkTunaiCV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AkTunaiRuncitId,KategoriPenerima,Tahun,NoCV,Tarikh,SuPekerjaId,AkPembekalId,Penerima,Alamat1,Alamat2,Almat3,Catatan,Jumlah,FlPosting,FlCetak,FlBatal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTunaiCV akTunaiCV)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akTunaiCV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akTunaiCV.AkPembekalId);
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id", akTunaiCV.AkTunaiRuncitId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id", akTunaiCV.SuPekerjaId);
            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
            if (akTunaiCV == null)
            {
                return NotFound();
            }
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akTunaiCV.AkPembekalId);
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id", akTunaiCV.AkTunaiRuncitId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id", akTunaiCV.SuPekerjaId);
            return View(akTunaiCV);
        }

        // POST: AkTunaiCV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AkTunaiRuncitId,KategoriPenerima,Tahun,NoCV,Tarikh,SuPekerjaId,AkPembekalId,Penerima,Alamat1,Alamat2,Almat3,Catatan,Jumlah,FlPosting,FlCetak,FlBatal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTunaiCV akTunaiCV)
        {
            if (id != akTunaiCV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akTunaiCV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTunaiCVExists(akTunaiCV.Id))
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
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akTunaiCV.AkPembekalId);
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id", akTunaiCV.AkTunaiRuncitId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id", akTunaiCV.SuPekerjaId);
            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _context.AkTunaiCV
                .Include(a => a.AkPembekal)
                .Include(a => a.AkTunaiRuncit)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTunaiCV == null)
            {
                return NotFound();
            }

            return View(akTunaiCV);
        }

        // POST: AkTunaiCV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
            _context.AkTunaiCV.Remove(akTunaiCV);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTunaiCVExists(int id)
        {
            return _context.AkTunaiCV.Any(e => e.Id == id);
        }
    }
}
