 using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JBahagianController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JBahagianController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JBahagian
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JBahagian.Include(j => j.JKW);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JBahagian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _context.JBahagian
                .Include(j => j.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBahagian == null)
            {
                return NotFound();
            }

            return View(jBahagian);
        }

        // GET: JBahagian/Create
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Kod == "100");

            var kumpulanWang = kw.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JBahagian
                        .IgnoreQueryFilters()
                        .Where(x=> x.JKW.Kod == kw.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list;
            return View();
        }

        [HttpPost]
        public JsonResult JsonGetKod(int data)
        {
            try
            {
                var result = "";
                if (data == 0)
                {
                    result = "";
                }
                else
                {
                    // get latest no rujukan running number  
                    var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = kw.Kod;
                    string prefix = kumpulanWang;
                    int x = 1;
                    string noRujukan = prefix + "00";

                    var LatestNoRujukan = _context.JBahagian
                                .IgnoreQueryFilters()
                                .Where(x => x.JKW.Kod == kw.Kod)
                                .Max(x => x.Kod);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(3));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }

                    result = noRujukan;
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // POST: JBahagian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JBahagian jBahagian, int JKWId)
        {
            JBahagian m = new JBahagian();
            var user = await _userManager.GetUserAsync(User);

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == JKWId);

            var kumpulanWang = kw.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JBahagian
                        .IgnoreQueryFilters()
                        .Where(x => x.JKW.Kod == kw.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            if (jBahagian != null && JKWId != 0)
            {
                
                if (ModelState.IsValid)
                {
                    m.JKWId = JKWId;
                    m.Kod = noRujukan;
                    m.Perihal = jBahagian.Perihal;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    _context.Add(jBahagian);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }

            List<JKW> list = _context.JKW.ToList();
            ViewBag.NoRujukan = noRujukan;
            ViewBag.JKw = list; 
            return View(jBahagian);
        }

        // GET: JBahagian/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _context.JBahagian.FindAsync(id);
            if (jBahagian == null)
            {
                return NotFound();
            }
            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list; 
            return View(jBahagian);
        }

        // POST: JBahagian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,JBahagian jBahagian, int JKWId)
        {
            if (id != jBahagian.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    JBahagian jBahagianAsal = await _context.JBahagian.FindAsync(id);

                    // list of input that cannot be change
                    jBahagian.JKWId = jBahagianAsal.JKWId;
                    jBahagian.Kod = jBahagianAsal.Kod;
                    jBahagian.TarMasuk = jBahagianAsal.TarMasuk;
                    jBahagian.UserId = jBahagianAsal.UserId;
                    // list of input that cannot be change end
                    _context.Entry(jBahagianAsal).State = EntityState.Detached;

                    jBahagian.UserIdKemaskini = user.UserName;
                    jBahagian.TarKemaskini = DateTime.Now;

                    _context.Update(jBahagian);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JBahagianExists(jBahagian.Id))
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
            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list; 
            return View(jBahagian);
        }

        // GET: JBahagian/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _context.JBahagian
                .Include(j => j.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBahagian == null)
            {
                return NotFound();
            }

            return View(jBahagian);
        }

        // POST: JBahagian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jBahagian = await _context.JBahagian.FindAsync(id);
            _context.JBahagian.Remove(jBahagian);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JBahagianExists(int id)
        {
            return _context.JBahagian.Any(e => e.Id == id);
        }
    }
}
