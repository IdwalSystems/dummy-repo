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
    public class AkPOController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int> _akPORepo;
        private readonly IRepository<AkPO1, int> _akPO1Repo;
        private readonly IRepository<AkPO2, int> _akPO2Repo;
        private readonly IRepository<AkPembekal, int> _akpembekalRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;
        private readonly IRepository<JBank, int> _jbankRepo;
        private readonly IRepository<JNegeri, int> _negeriRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private CartPO _cart;

        public AkPOController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int> AkPORepository,
            IRepository<AkPO1, int> AkPO1Repository,
            IRepository<AkPO2, int> AkPO2Repository,
            IRepository<AkPembekal, int> AkPembekalRepository,
            IRepository<AkBank, int> akBankRepository,
            IRepository<JBank, int> JBankRepository,
            IRepository<JNegeri, int> negeriRepository,
            IRepository<JKW, int> kwRepository,
            CartPO cart
            )
        {
            _context = context;
            _userManager = userManager;
            _akPORepo = AkPORepository;
            _akPO1Repo = AkPO1Repository;
            _akPO2Repo = AkPO2Repository;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akpembekalRepo = AkPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _cart = cart;
        }

        // GET: AkPO
        public async Task<IActionResult> Index()
        {
            var akPO = await _akPORepo.GetAll();
            return View(akPO);
        }

        // GET: AkPO/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;
            var negeri = await _negeriRepo.GetById(akPO.AkPembekal.JNegeriId);
            akPO.AkPembekal.JNegeri = negeri;
            if (akPO == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akPO);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkPembekal> PembekalList = _context.AkPembekal.OrderBy(b => b.Id).ToList();
            ViewBag.AkPembekal = PembekalList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW).OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta = akCartaList;

            List<JCaraBayar> jCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = jCaraBayarList;

        }

        private void PopulateTable(int? id)
        {
            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO1 = akPO1Table;

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO2 = akPO2Table;
        }
        private void PopulateCart(AkPO akPO)
        {
            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO1 akPO1 in akPO1Table)
            {
                _cart.AddItem1(akPO1.AkPOId,
                               akPO1.Amaun,
                               akPO1.AkCartaId);
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.JCaraBayar)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO2 akPO2 in akPO2Table)
            {
                _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
                               akPO2.Bil,
                               akPO2.NoStok,
                               akPO2.Perihal,
                               akPO2.Kuantiti,
                               akPO2.Unit,
                               akPO2.Harga,
                               akPO2.Amaun);
            }
        }

        // GET: AkPO/Create
        public IActionResult Create()
        {
            PopulateList();
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "KodSykt", "NamaSykt");
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            return View();
        }

        // POST: AkPO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoPO,Tarikh,TarikhPosting,AkPembekalId,Jumlah,Posting,JKWId,Tahun,Batal")] AkPO akPO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akPO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "Id", akPO.AkPembekalId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akPO.JKWId);
            return View(akPO);
        }

        // GET: AkPO/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _context.AkPO.FindAsync(id);
            if (akPO == null)
            {
                return NotFound();
            }
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "Id", akPO.AkPembekalId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akPO.JKWId);
            return View(akPO);
        }

        // POST: AkPO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoPO,Tarikh,TarikhPosting,AkPembekalId,Jumlah,Posting,JKWId,Tahun,Batal")] AkPO akPO)
        {
            if (id != akPO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akPO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPOExists(akPO.Id))
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
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "Id", akPO.AkPembekalId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akPO.JKWId);
            return View(akPO);
        }

        // GET: AkPO/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _context.AkPO
                .Include(a => a.AkPembekal)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPO == null)
            {
                return NotFound();
            }

            return View(akPO);
        }

        // POST: AkPO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPO = await _context.AkPO.FindAsync(id);
            _context.AkPO.Remove(akPO);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkPOExists(int id)
        {
            return _context.AkPO.Any(e => e.Id == id);
        }
    }
}
