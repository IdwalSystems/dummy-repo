using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Helper;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{

    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JKonfigPenyataController : Controller
    {
        public const string modul = "JD015";
        public const string namamodul = "Jadual Konfigurasi Penyata Aliran Tunai";
        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CartJKonfigPenyata _cart;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<JKonfigPenyata, int, string> _penyataRepo;
        private readonly CustomIRepository<string, int> _customRepo;

        public JKonfigPenyataController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            CartJKonfigPenyata cart,
            IRepository<AkCarta, int, string> akCartaRepo,
            IRepository<JKonfigPenyata, int, string> penyataRepo,
            CustomIRepository<string, int> customRepo

            )
        {
            _penyataRepo = penyataRepo;
            _customRepo = customRepo;
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _cart = cart;
            _akCartaRepo = akCartaRepo;
        }
        private async Task AddLogAsync(
            string operasi,
            string nota,
            string rujukan,
            int idRujukan,
            decimal jumlah,
            int? pekerjaId)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;
            appLog.SuPekerjaId = pekerjaId;

            await _appLog.Insert(appLog, modul, operasi);
        }

        public IActionResult Index()
        {
            return View(_context.JKonfigPenyata.ToList());
        }

        //[Authorize(Policy = modul)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konfigPenyata = await _penyataRepo.GetByIdIncludeDeletedItems((int)id);

            if (konfigPenyata == null)
            {
                return NotFound();
            }

            return View(konfigPenyata);
        }

        //[Authorize(Policy = modul + "C")]
        public IActionResult Create()
        {
            EmptyCart();
            PopulateDropdownList();
            return View();
        }

        private void PopulateDropdownList()
        {
            var kategoriTajuk = EnumHelper<EnKategoriTajuk>.GetList();
            ViewBag.EnKategoriTajuk = kategoriTajuk;

            var kategoriJumlah = EnumHelper<EnKategoriJumlah>.GetList();
            ViewBag.EnKategoriJumlah = kategoriJumlah;

            var jenisCarta = EnumHelper<EnJenisCarta>.GetList();

            ViewBag.EnJenisCartaList = jenisCarta;

            ViewBag.KodList = _context.AkCarta.Where(c => c.JParasId == 3).OrderBy(c => c.Kod).ToList(); // paras 4
        }

        private JsonResult EmptyCart()
        {
            try
            {
                _cart.ClearBaris();
                _cart.ClearBarisFormula();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult EmptyBarisCart()
        {
            try
            {
                _cart.ClearBarisFormulaByBarisBil();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = modul + "C")]
        public async Task<IActionResult> Create(JKonfigPenyata konfigPenyata, string syscode)
        {
            if (konfigPenyata.Tahun != null && !TahunKodPenyataExists(konfigPenyata.Tahun, konfigPenyata.Kod))
            {
                if (ModelState.IsValid)
                {
                    if (_cart.JKonfigPenyataBaris != null && _cart.JKonfigPenyataBaris.Any())
                    {
                        foreach (var baris in _cart.JKonfigPenyataBaris)
                        {
                            if (baris.JKonfigPenyataBarisFormula != null && baris.JKonfigPenyataBarisFormula.Any())
                            {
                                foreach (var formula in baris.JKonfigPenyataBarisFormula)
                                {
                                    formula.Id = 0;
                                    formula.JKonfigPenyataBarisId = 0;
                                }
                            }

                            baris.Id = 0;
                            baris.JKonfigPenyataId = 0;
                        }
                    }
                    konfigPenyata.JKonfigPenyataBaris = _cart.JKonfigPenyataBaris?.ToList();

                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    konfigPenyata.UserId = user?.UserName ?? "";

                    konfigPenyata.TarMasuk = DateTime.Now;
                    konfigPenyata.DPekerjaMasukId = pekerjaId;

                    _context.Add(konfigPenyata);
                    await AddLogAsync("Tambah", konfigPenyata.Kod + " - " + konfigPenyata.Tahun, konfigPenyata.Kod + " - " + konfigPenyata.Tahun ?? "", 0, 0, pekerjaId);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                TempData[SD.Error] = "Tahun untuk kod laporan ini telah wujud..!";
            }

            PopulateDropdownList();
            return View(konfigPenyata);
        }

        private bool TahunKodPenyataExists(string tahun, string kod)
        {
            return _context.JKonfigPenyata.Any(kp => kp.Tahun == tahun && kp.Kod == kod);
        }

        // GET: KonfigPenyata/Edit/5
        //[Authorize(Policy = modul + "E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konfigPenyata = await _penyataRepo.GetById((int)id);
            if (konfigPenyata == null)
            {
                return NotFound();
            }
            PopulateDropdownList();
            EmptyCart();
            PopulateCartJKonfigPenyataFromDb(konfigPenyata);
            return View(konfigPenyata);
        }

        private void PopulateCartJKonfigPenyataFromDb(JKonfigPenyata konfigPenyata)
        {
            if (konfigPenyata.JKonfigPenyataBaris != null)
            {
                foreach (var baris in konfigPenyata.JKonfigPenyataBaris)
                {
                    var formula = new List<JKonfigPenyataBarisFormula>();

                    if (baris.JKonfigPenyataBarisFormula != null && baris.JKonfigPenyataBarisFormula.Count > 0)
                    {
                        foreach (var foo in baris.JKonfigPenyataBarisFormula)
                        {
                            foo.JKonfigPenyataBaris = null;
                        }

                        formula.AddRange(baris.JKonfigPenyataBarisFormula);
                    }
                    baris.JKonfigPenyata = null;

                    _cart.AddItemBaris(baris.Id, baris.Bil, baris.JKonfigPenyataId, baris.EnKategoriTajuk, baris.Perihal, baris.Susunan, baris.IsFormula, baris.EnKategoriJumlah, baris.JumlahSusunanList, formula);
                }
            }
            PopulateListViewFromCart();
        }

        private void PopulateListViewFromCart()
        {
            List<JKonfigPenyataBaris> baris = _cart.JKonfigPenyataBaris.ToList();

            ViewBag.JKonfigPenyataBaris = baris.OrderBy(b => b.Susunan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = modul + "E")]
        public async Task<IActionResult> Edit(int id, JKonfigPenyata konfigPenyata, string syscode)
        {
            if (id != konfigPenyata.Id)
            {
                return NotFound();
            }

            if (konfigPenyata.Tahun != null && ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

                    var objAsal = await _penyataRepo.GetById(konfigPenyata.Id);
                    var tahunAsal = objAsal?.Tahun;
                    if (objAsal != null)
                    {
                        konfigPenyata.UserId = objAsal.UserId;
                        konfigPenyata.TarMasuk = objAsal.TarMasuk;
                        konfigPenyata.DPekerjaMasukId = objAsal.DPekerjaMasukId;

                        //if (objAsal.JKonfigPenyataBaris != null && objAsal.JKonfigPenyataBaris.Count > 0)
                        //{
                        //    foreach (var item in objAsal.JKonfigPenyataBaris)
                        //    {
                        //        if (item.JKonfigPenyataBarisFormula != null && item.JKonfigPenyataBarisFormula.Count > 0)
                        //        {
                        //            foreach (var foo in item.JKonfigPenyataBarisFormula)
                        //            {
                        //                var formula = _context.JKonfigPenyataBarisFormula.FirstOrDefault(f => f.Id == foo.Id);
                        //                if (formula != null) _context.Remove(foo);
                        //            }

                        //        }

                        //        var model = _context.JKonfigPenyataBaris.FirstOrDefault(b => b.Id == item.Id);
                        //        if (model != null) _context.Remove(model);
                        //    }
                        //}

                        _context.Entry(objAsal).State = EntityState.Detached;
                    }

                    //konfigPenyata.JKonfigPenyataBaris = _cart.JKonfigPenyataBaris?.ToList();
                    konfigPenyata.UserIdKemaskini = user?.UserName ?? "";

                    konfigPenyata.TarKemaskini = DateTime.Now;
                    konfigPenyata.DPekerjaKemaskiniId = pekerjaId;

                    _context.Update(konfigPenyata);

                    await AddLogAsync("Ubah", konfigPenyata.Kod + " - " + konfigPenyata.Tahun, konfigPenyata.Kod + " - " + konfigPenyata.Tahun ?? "", konfigPenyata.Id, 0, pekerjaId);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonfigPenyataExists(konfigPenyata.Id))
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
            PopulateDropdownList();
            return View(konfigPenyata);
        }

        private bool KonfigPenyataExists(int id)
        {
            return _context.JKonfigPenyata.Any(b => b.Id == id);
        }

        //[Authorize(Policy = modul + "D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var konfigPenyata = await _penyataRepo.GetById((int)id);

            if (konfigPenyata == null) return NotFound();

            return View(konfigPenyata);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = modul + "D")]
        public async Task<IActionResult> DeleteConfirmed(int id, string syscode)
        {
            var konfigPenyata = await _context.JKonfigPenyata.FindAsync((int)id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            if (konfigPenyata != null && konfigPenyata.Tahun != null)
            {
                konfigPenyata.UserIdKemaskini = user?.UserName ?? "";
                konfigPenyata.TarKemaskini = DateTime.Now;
                konfigPenyata.DPekerjaKemaskiniId = pekerjaId;

                _context.JKonfigPenyata.Remove(konfigPenyata);
                await AddLogAsync("Hapus", konfigPenyata.Kod + " - " + konfigPenyata.Tahun, konfigPenyata.Kod + " - " + konfigPenyata.Tahun ?? "", konfigPenyata.Id, 0, pekerjaId);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dihapuskan..!";
            }

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Policy = modul + "R")]
        public async Task<IActionResult> RollBack(int id, string syscode)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user!.Id).FirstOrDefault()!.SuPekerjaId;

            var obj = await _context.JKonfigPenyata.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            if (obj != null)
            {
                obj.FlHapus = 0;
                obj.UserIdKemaskini = user?.UserName ?? "";
                obj.TarKemaskini = DateTime.Now;
                obj.DPekerjaKemaskiniId = pekerjaId;

                _context.JKonfigPenyata.Update(obj);

                // Batal operation end
                await AddLogAsync("Rollback", obj.Kod + " - " + obj.Tahun, obj.Kod + " - " + obj.Tahun ?? "", obj.Id, 0, pekerjaId);

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dikembalikan..!";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetItemsBasedOnYear(string tahun, string kod)
        {
            try
            {
                var result = await _customRepo.GetAllDetailsByTahunOrKod(tahun, kod);

                if (result.Id == 0)
                {
                    result = await _customRepo.GetAllDetailsByTahunOrKod((int.Parse(tahun ?? DateTime.Now.Year.ToString()) - 1).ToString(), kod);
                    if (result.Id == 0)
                    {
                        result = await _customRepo.GetAllDetailsByTahunOrKod(tahun, null);
                        if (result.Id == 0)
                        {
                            result = await _customRepo.GetAllDetailsByTahunOrKod((int.Parse(tahun ?? DateTime.Now.Year.ToString()) - 1).ToString(), null);
                        }
                    }
                }

                if (result != null)
                {
                    PopulateCartJKonfigPenyataFromDb(result);
                }

                return Json(new { result = "OK", record = result });

            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }



        }

        public JsonResult RemoveCartJKonfigPerubahanEkuitiBaris(JKonfigPenyataBaris baris)
        {
            try
            {
                _cart.RemoveItemBaris(baris.Bil);

                // remove from db
                var obj = _context.JKonfigPenyataBaris.Find(baris.Id);

                if (obj != null)
                {
                    _context.Remove(obj);
                }

                _context.SaveChanges();

                // remove from db end
                
                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAllItemCartJKonfigPenyata(int jKonfigPenyataId)
        {

            try
            {
                // if id = 0, get from db
                if (jKonfigPenyataId != 0)
                {
                    EmptyCart();

                    var list = _context.JKonfigPenyataBaris.Include(b => b.JKonfigPenyataBarisFormula)
                        .Where(b => b.JKonfigPenyataId == jKonfigPenyataId).ToList();

                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            var formula = new List<JKonfigPenyataBarisFormula>();

                            if (item.JKonfigPenyataBarisFormula != null && item.JKonfigPenyataBarisFormula.Count > 0)
                            {
                                foreach (var foo in item.JKonfigPenyataBarisFormula)
                                {
                                    foo.JKonfigPenyataBaris = null;
                                }

                                formula.AddRange(item.JKonfigPenyataBarisFormula);
                            }
                            item.JKonfigPenyata = null;

                            _cart.AddItemBaris(item.Id, item.Bil, item.JKonfigPenyataId, item.EnKategoriTajuk, item.Perihal, item.Susunan, item.IsFormula, item.EnKategoriJumlah, item.JumlahSusunanList, formula);
                        }
                    }
                }
                // get from db end

                List<JKonfigPenyataBaris> baris = _cart.JKonfigPenyataBaris.ToList();

                foreach (var bar in baris)
                {
                    //bar.JKonfigPenyataBarisFormula = _cartBaris.JKonfigPenyataBarisFormula.Where(x => x.BarisBil == bar.Bil).OrderBy(f => f.BarisBil).ToList();
                }

                return Json(new { result = "OK", baris = baris.OrderBy(d => d.Susunan) });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAnItemFromCartJKonfigPenyataBaris(JKonfigPenyataBaris baris)
        {

            try
            {
                JKonfigPenyataBaris data = _cart.JKonfigPenyataBaris.FirstOrDefault(x => x.Bil == baris.Bil) ?? new JKonfigPenyataBaris();

                if (data != null && data.JKonfigPenyataBarisFormula != null && data.JKonfigPenyataBarisFormula.Count > 0)
                {
                    foreach (var formula in data.JKonfigPenyataBarisFormula)
                    {
                        _cart.AddItemBarisFormula(formula.Id, formula.BarisBil, formula.JKonfigPenyataBarisId, formula.EnJenisOperasi, formula.IsPukal, formula.EnJenisCartaList, formula.IsKecuali, formula.KodList, formula.SetKodList, formula.AmaunTetap, formula.IsLastYear, formula.IsUntilYear);
                    }
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetBilJKonfigPenyataBaris(int JKonfigPenyataId)
        {
            try
            {
                int? bil = _cart.JKonfigPenyataBaris.OrderByDescending(b => b.Bil).FirstOrDefault()?.Bil ?? 0;

                bil += 1;

                // add baris into db
                var obj = _context.JKonfigPenyataBaris.FirstOrDefault(b => b.Bil == bil && b.JKonfigPenyataId == JKonfigPenyataId);
                int? susunan = _context.JKonfigPenyataBaris.Where(b => b.JKonfigPenyataId == JKonfigPenyataId).OrderByDescending(b => b.Susunan).FirstOrDefault()?.Susunan + 1 ?? 1;
                if (obj == null)
                {
                    _context.Add(new JKonfigPenyataBaris
                    {
                        Bil = (int)bil,
                        JKonfigPenyataId = JKonfigPenyataId,
                        EnKategoriTajuk = EnKategoriTajuk.TajukUtama,
                        Perihal = "PERIHALAN BARU",
                        Susunan = (int)susunan,
                        IsFormula = false,
                        EnKategoriJumlah = EnKategoriJumlah.Amaun,
                        JumlahSusunanList = ""
                    });

                    _context.SaveChanges();
                }
                // add baris into db end
                
                return Json(new { result = "OK", bil });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public JsonResult SaveAnItemFromCartJKonfigPenyataBaris(JKonfigPenyataBaris baris)
        {
            try
            {
                var data = _cart.JKonfigPenyataBaris.FirstOrDefault(x => x.Bil == baris.Bil);
                int? susunan = null;
                if (data != null)
                {
                    susunan = data.Susunan;
                    var obj = _context.JKonfigPenyataBaris.Find(baris.Id);

                    if (obj != null)
                    {
                        obj.Bil = baris.Bil;
                        obj.JKonfigPenyataId = baris.JKonfigPenyataId;
                        obj.EnKategoriTajuk = baris.EnKategoriTajuk;
                        obj.Perihal = baris.Perihal;
                        obj.Susunan = baris.Susunan;
                        obj.IsFormula = baris.IsFormula;
                        obj.EnKategoriJumlah = baris.EnKategoriJumlah;
                        obj.JumlahSusunanList = baris.JumlahSusunanList;

                        _context.Update(obj);
                    }
                    
                }

                _context.SaveChanges();

                return Json(new { result = "OK", susunan });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetBilJKonfigPenyataBarisFormula(int JKonfigPenyataBarisId)
        {
            try
            {
                int? bil = _cart.JKonfigPenyataBarisFormula.OrderByDescending(b => b.BarisBil).FirstOrDefault()?.BarisBil ?? 0;

                bil += 1;

                // add baris into db
                var obj = _context.JKonfigPenyataBarisFormula.FirstOrDefault(b => b.BarisBil == bil && b.JKonfigPenyataBarisId == JKonfigPenyataBarisId);

                if (obj == null)
                {
                    _context.Add(new JKonfigPenyataBarisFormula
                    {
                        BarisBil = (int)bil,
                        JKonfigPenyataBarisId = JKonfigPenyataBarisId,
                        EnJenisOperasi = EnJenisOperasi.Tambah,
                        IsPukal = false,
                        EnJenisCartaList = "",
                        IsKecuali = false,
                        KodList = "",
                        SetKodList = "",
                        AmaunTetap = 0,
                        IsLastYear = false,
                        IsUntilYear = false,
                    });

                    _context.SaveChanges();
                }
                // add baris into db end

                return Json(new { result = "OK", bil });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveCartJKonfigPerubahanEkuitiBarisFormula(JKonfigPenyataBarisFormula formula)
        {
            try
            {
                _cart.RemoveItemBarisFormula(formula.Id, formula.BarisBil);

                // remove from db
                var obj = _context.JKonfigPenyataBarisFormula.Find(formula.Id);

                if (obj != null)
                {
                    _context.Remove(obj);
                }

                _context.SaveChanges();

                // remove from db end

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAnItemFromCartJKonfigPenyataBarisFormula(JKonfigPenyataBarisFormula formula)
        {

            try
            {
                JKonfigPenyataBarisFormula data = _cart.JKonfigPenyataBarisFormula.FirstOrDefault(x => x.Id == formula.Id) ?? new JKonfigPenyataBarisFormula();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAnItemFromCartJKonfigPenyataBarisFormula(JKonfigPenyataBarisFormula formula)
        {
            try
            {
                var data = _cart.JKonfigPenyataBarisFormula.FirstOrDefault(x => x.Id == formula.Id);
                int? bil = _cart.JKonfigPenyataBarisFormula.OrderByDescending(b => b.BarisBil).FirstOrDefault()?.BarisBil ?? 0;

                bil += 1;

                formula.SetKodList = formula.KodList;

                if (data != null)
                {
                    var obj = _context.JKonfigPenyataBarisFormula.Find(formula.Id);

                    if (obj != null)
                    {
                        obj.BarisBil = formula.BarisBil;
                        obj.JKonfigPenyataBarisId = formula.JKonfigPenyataBarisId;
                        obj.EnJenisOperasi = formula.EnJenisOperasi;
                        obj.IsPukal = formula.IsPukal;
                        obj.EnJenisCartaList = formula.EnJenisCartaList;
                        obj.IsKecuali = formula.IsKecuali;
                        obj.KodList = formula.KodList;
                        obj.SetKodList = formula.SetKodList;
                        obj.AmaunTetap = formula.AmaunTetap;
                        obj.IsLastYear = formula.IsLastYear;
                        obj.IsUntilYear = formula.IsUntilYear;

                        _context.Update(obj);
                    }

                    _context.SaveChanges();
                }

                //if (data != null)
                //{
                //    bil = formula.BarisBil;
                //    _cart.RemoveItemBarisFormula(formula.EnJenisOperasi, formula.BarisBil);

                //    _cart.AddItemBarisFormula(formula.Id, (int)bil, formula.JKonfigPenyataBarisId, formula.EnJenisOperasi, formula.IsPukal, formula.EnJenisCartaList, formula.IsKecuali, formula.KodList, formula.SetKodList, formula.AmaunTetap, formula.IsLastYear);
                //}
                //else
                //{
                //    _cart.AddItemBarisFormula(formula.Id, (int)bil, formula.JKonfigPenyataBarisId, formula.EnJenisOperasi, formula.IsPukal, formula.EnJenisCartaList, formula.IsKecuali, formula.KodList, formula.SetKodList, formula.AmaunTetap, formula.IsLastYear);
                //}

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAllItemCartJKonfigPenyataBaris(int jKonfigPenyataBarisId)
        {

            try
            {

                // if id = 0, get from db
                if (jKonfigPenyataBarisId != 0)
                {
                    EmptyBarisCart();

                    var list = _context.JKonfigPenyataBarisFormula
                        .Where(b => b.JKonfigPenyataBarisId == jKonfigPenyataBarisId).ToList();

                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            _cart.AddItemBarisFormula(item.Id,item.BarisBil, item.JKonfigPenyataBarisId, item.EnJenisOperasi, item.IsPukal, item.EnJenisCartaList, item.IsKecuali, item.KodList, item.SetKodList, item.AmaunTetap, item.IsLastYear, item.IsUntilYear);
                        }
                    }
                }
                // get from db end

                List<JKonfigPenyataBarisFormula> formula = _cart.JKonfigPenyataBarisFormula.ToList();

                foreach (var item in formula.OrderBy(b => b.BarisBil).ThenBy(b => b.EnJenisOperasi))
                {

                    string sentence = _akCartaRepo.FormulaInSentence(item.EnJenisOperasi, item.EnJenisCartaList, item.IsKecuali, item.KodList,item.AmaunTetap, item.IsLastYear);

                    if (item.IsUntilYear) sentence += "(kiraan sehingga tahun)";
                    item.BarisDescription = "Operasi " + item.EnJenisOperasi.GetDisplayName();
                    item.FormulaDescription = sentence;
                }

                return Json(new { result = "OK", formula = formula.OrderBy(d => d.BarisBil) });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
    }
}
