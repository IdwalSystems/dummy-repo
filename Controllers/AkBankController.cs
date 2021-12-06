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
using MSNK.Models.Modules.ViewModel;

namespace MSNK.Controllers
{
    [Authorize]
    public class AkBankController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<JBank, int> _bankRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;

        public AkBankController(ApplicationDbContext context,
                                IRepository<JKW, int> kwRepository,
                                IRepository<JBank, int> bankRepository,
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
            List<JBank> bankList = _context.JBank.ToList();
            bankList.Insert(0, new JBank { Id = 0, Nama = "-- Pilih Bank --" });
            ViewBag.JBank = bankList;
        }
        private void PopulateKWList()
        {
            List<JKW> kwList = _context.JKW.ToList();
            kwList.Insert(0, new JKW { Id = 0, Perihal = "-- Pilih Kumpulan Wang --" });
            ViewBag.JKW = kwList;
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
        public async Task<IActionResult> Create(AkBankViewModel akBank, int JKWId, int JBankId)
        {
            AkBank akB = new AkBank();
            if (ModelState.IsValid)
            {
                if (akBank != null && JKWId != 0 && JBankId != 0)
                {
                    akB.JBankId = JBankId;
                    akB.JKWId = JKWId;
                    akB.Kod = akBank.Kod;
                    akB.NoAkaun = akBank.NoAkaun;
                    await _akBankRepo.Insert(akB);
                    await _akBankRepo.Save();

                    return RedirectToAction(nameof(Index));
                }
                
            }

            PopulateKWList();
            PopulateBankList();
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

            PopulateBankList();
            PopulateKWList();

            return View(akBank);
        }

        // POST: AkBank/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkBank akBank, int KWId, int BankId)
        {
            if (id != akBank.Id)
            {
                return NotFound();
            }

            AkBank akB = new AkBank();

            if (ModelState.IsValid)
            {
                try
                {
                    akB.JBankId = BankId;
                    akB.JKWId = KWId;
                    akB.Kod = akBank.Kod;
                    akB.NoAkaun = akBank.NoAkaun;
                    await _akBankRepo.Update(akB);
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
            PopulateKWList();
            PopulateBankList();

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
