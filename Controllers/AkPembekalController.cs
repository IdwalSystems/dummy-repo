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
    public class AkPembekalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkPembekal, int> _akpembekalRepo;
        private readonly IRepository<JBank, int> _jbankRepo;
        private readonly IRepository<JNegeri, int> _jnegeriRepo;

        public AkPembekalController(
            ApplicationDbContext context,
            IRepository<AkPembekal, int> AkPembekalRepository,
            IRepository<JBank, int> JBankRepository,
            IRepository<JNegeri, int> JNegeriRepository)
        {
            _context = context;
            _akpembekalRepo = AkPembekalRepository;
            _jbankRepo = JBankRepository;
            _jnegeriRepo = JNegeriRepository;
        }

        private void PopulateList()
        {
            List<JBank> JBankList = _context.JBank.OrderBy(b => b.Kod).ToList();
            List<JNegeri> jnegeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();

            ViewBag.jbank = JBankList;
            ViewBag.jnegeri = jnegeriList;
        }

        private string GetKodSykt(string namasykt)
        {
            var akpembekal = _akpembekalRepo.GetAll()
                .Result
                .Where(s => s.KodSykt.Contains(namasykt.Substring(0, 1)))
                .OrderByDescending(s => s.Id).FirstOrDefault();

            int intkodsykt = 0;
            if (akpembekal != null)
            {
                if (int.TryParse(akpembekal.KodSykt.Substring(1), out intkodsykt))
                {
                    intkodsykt += 1;
                }
            }
            else
            {
                intkodsykt = 1;
            }

            string newkodsykt = namasykt.Substring(0, 1) + intkodsykt.ToString("D5");
            ViewBag.kodsykt = newkodsykt;
            return newkodsykt;
        }

        // GET: AkPembekal
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.NamaSyktSortParm = String.IsNullOrEmpty(sortOrder) ? "nama_desc" : "";
            ViewBag.KodSyktSortParm = sortOrder == "kod" ? "kod_desc" : "kod";

            var akpembekal = await _akpembekalRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                akpembekal = akpembekal.Where(s => s.KodSykt.Contains(searchString) || s.NamaSykt.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "nama_desc":
                    akpembekal = akpembekal.OrderByDescending(s => s.NamaSykt);
                    break;
                case "kod":
                    akpembekal = akpembekal.OrderBy(s => s.KodSykt);
                    break;
                case "kod_desc":
                    akpembekal = akpembekal.OrderByDescending(s => s.KodSykt);
                    break;
                default:
                    akpembekal = akpembekal.OrderBy(s => s.NamaSykt);
                    break;
            }
            return View(akpembekal);
        }

        // GET: AkPembekal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var akPembekal = await _context.AkPembekal
            //    .Include(a => a.JBank)
            //    .Include(a => a.JNegeri)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var akPembekal = await _akpembekalRepo.GetById((int)id);
            var bank = await _jbankRepo.GetById(akPembekal.JBankId);
            akPembekal.JBank = bank;
            var negeri = await _jnegeriRepo.GetById(akPembekal.JNegeriId);
            akPembekal.JNegeri = negeri;

            if (akPembekal == null)
            {
                return NotFound();
            }

            return View(akPembekal);
        }

        // GET: AkPembekal/Create
        public IActionResult Create()
        {
            //ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id");
            //ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod");
            PopulateList();
            return View();
        }

        // POST: AkPembekal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPembekal akPembekal, int jNegeriId, int jBankId)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(akPembekal);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["JBankId"] = new SelectList(_context.JBank, "Id", "Id", akPembekal.JBankId);
            //ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            //return View(akPembekal);
            AkPembekal akP = new();
            if (ModelState.IsValid)
            {
                if (akPembekal != null)
                {
                    akP.JBankId = jBankId;
                    akP.JNegeriId = jNegeriId;
                    akP.KodSykt = GetKodSykt(akPembekal.NamaSykt);
                    akP.NamaSykt = akPembekal.NamaSykt;
                    akP.NoPendaftaran = akPembekal.NoPendaftaran;
                    akP.Poskod = akPembekal.Poskod;
                    akP.Telefon1 = akPembekal.Telefon1;
                    akP.AkaunBank = akPembekal.AkaunBank;
                    akP.Alamat1 = akPembekal.Alamat1;
                    akP.Alamat2 = akPembekal.Alamat2;
                    akP.Alamat3 = akPembekal.Alamat3;
                    akP.Bandar = akPembekal.Bandar;
                    akP.Emel = akPembekal.Emel;
                    await _akpembekalRepo.Insert(akP);
                    await _akpembekalRepo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();

            return View(akP);
        }

        // GET: AkPembekal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var akPembekal = await _context.AkPembekal.FindAsync(id);
            PopulateList();
            var akPembekal = await _akpembekalRepo.GetById((int)id);
            var bank = await _jbankRepo.GetById(akPembekal.JBankId);
            akPembekal.JBank = bank;
            var negeri = await _jnegeriRepo.GetById(akPembekal.JNegeriId);
            akPembekal.JNegeri = negeri;
            if (akPembekal == null)
            {
                return NotFound();
            }
            //ViewData["JBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.JBankId);
            //ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // POST: AkPembekal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkPembekal akPembekal, int jNegeriId, int jBankId)
        {
            if (id != akPembekal.Id)
            {
                return NotFound();
            }

            //AkPembekal akP = new();

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(akPembekal);
                    //await _context.SaveChangesAsync();

                    //akP.JBankId = jBankId;
                    //akP.JNegeriId = jNegeriId;
                    //akP.KodSykt = akPembekal.KodSykt;
                    //akP.NamaSykt = akPembekal.NamaSykt;
                    //akP.NoPendaftaran = akPembekal.NoPendaftaran;
                    //akP.Poskod = akPembekal.Poskod;
                    //akP.Telefon1 = akPembekal.Telefon1;
                    //akP.AkaunBank = akPembekal.AkaunBank;
                    //akP.Alamat1 = akPembekal.Alamat1;
                    //akP.Alamat2 = akPembekal.Alamat2;
                    //akP.Alamat3 = akPembekal.Alamat3;
                    //akP.Bandar = akPembekal.Bandar;
                    //akP.Emel = akPembekal.Emel;
                    await _akpembekalRepo.Update(akPembekal);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPembekalExists(akPembekal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                PopulateList();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["JBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.JBankId);
            //ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // GET: AkPembekal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPembekal = await _context.AkPembekal
                .Include(a => a.JBank)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPembekal == null)
            {
                return NotFound();
            }

            return View(akPembekal);
        }

        // POST: AkPembekal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPembekal = await _context.AkPembekal.FindAsync(id);
            _context.AkPembekal.Remove(akPembekal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkPembekalExists(int id)
        {
            return _context.AkPembekal.Any(e => e.Id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
