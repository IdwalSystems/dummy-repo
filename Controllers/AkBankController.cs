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
    public class AkBankController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<KW, int> _kwRepo;
        private readonly IRepository<Bank, int> _bankRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;

        public AkBankController(ApplicationDbContext context,
                                IRepository<KW, int> kwRepository,
                                IRepository<Bank, int> bankRepository,
                                IRepository<AkBank, int> akBankRepository)
        {
            _context = context;
            _kwRepo = kwRepository;
            _bankRepo = bankRepository;
            _akBankRepo = akBankRepository; 
        }

        // GET: AkBank
        public async Task<IActionResult> Index()
        {
            var akBank = await _akBankRepo.GetAll();

           return View(akBank);
            
        }

        // GET: AkBank/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBank = await _akBankRepo.GetById((int)id);
            if (akBank == null)
            {
                return NotFound();
            }

            return View(akBank);
        }
        
        private void PopulateBankList()
        {
            List<Bank> bankList = _context.Bank.ToList();
            bankList.Insert(0, new Bank { Id = 0, Nama = "-- Pilih Bank --" });
            ViewBag.Bank = bankList;
        }
        private void PopulateKWList()
        {
            List<KW> kwList = _context.KW.ToList();
            kwList.Insert(0, new KW { Id = 0, Perihal = "-- Pilih Kumpulan Wang --" });
            ViewBag.Kw = kwList;
        }
        // GET: AkBank/Create
        public IActionResult Create()
        {
            PopulateBankList();
            PopulateKWList();

            return View();
        }

        // POST: AkBank/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind()] AkBank akBank)
        {
            if (ModelState.IsValid)
            {
                await _akBankRepo.Insert(akBank);
                await _akBankRepo.Save();

                return RedirectToAction(nameof(Index));
            }
            
            return View(akBank);
        }

        // GET: AkBank/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBank = await _akBankRepo.GetById((int)id);
            if (akBank == null)
            {
                return NotFound();
            }
            return View(akBank);
        }

        // POST: AkBank/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind()] AkBank akBank)
        {
            if (id != akBank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _akBankRepo.Update(akBank);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBankExists(akBank.Id))
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
            return View(akBank);
        }

        // GET: AkBank/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBank = await _akBankRepo.GetById((int)id);
            if (akBank == null)
            {
                return NotFound();
            }

            return View(akBank);
        }

        // POST: AkBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _akBankRepo.Delete(id);
            await _akBankRepo.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool AkBankExists(int id)
        {
            return _context.AkBank.Any(e => e.Id == id);
        }
    }
}
