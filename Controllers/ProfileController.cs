using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MSNK.Data;
using MSNK.Models.Login.ViewModel;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMemoryCache _cache;

        public ProfileController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog,
            IWebHostEnvironment hostEnvironment,
            IMemoryCache cache)
        {
            _db = db;
            _userManager = userManager;
            _appLog = appLog;
            webHostEnvironment = hostEnvironment;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _db.applicationUsers.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            ResetPasswordViewModel viewModel = new ResetPasswordViewModel();

            viewModel.Email = user.Email;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ProfileSetting()
        {
            var user = await _db.applicationUsers.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            ApplicationUserViewModel viewModel = new ApplicationUserViewModel();
            viewModel.id = user.Id;
            viewModel.Nama = Regex.Replace(user.Nama, "[^a-zA-Z0-9_]+", "");
            viewModel.Id = user.Id;
            viewModel.GambarSediaAda = user.Tandatangan;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilSetting(string id, ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = await _db.applicationUsers.FirstOrDefaultAsync(x => x.Id == model.id);
                if (model.Gambar != null)
                {
                    if (model.GambarSediaAda != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "img\\signature", model.GambarSediaAda);

                        if (Directory.Exists(filePath))
                        {
                            var image = Image.FromFile(filePath);

                            image.Dispose();

                            System.IO.File.Delete(filePath);
                        }
                        
                    }
                    
                }
                obj.Tandatangan = ProcessUploadedFile(model);

                _db.Update(obj);
                await _db.SaveChangesAsync();
                TempData[SD.Success] = "Kemaskini tandatangan berjaya";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            TempData[SD.Error] = "Kemaskini tandatangan gagal";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private string ProcessUploadedFile(ApplicationUserViewModel model)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img\\signature");
            string str = Regex.Replace(model.Nama, "[^a-zA-Z0-9_]+", "");
            uniqueFileName = str + ".png";
            //uniqueFileName = model.Gambar.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Gambar.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor,User")]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    model.Code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        TempData[SD.Success] = "Tukar Katalaluan berjaya..!";
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    AddErrors(result);
                }  

            }

            TempData[SD.Error] = "Tukar Katalaluan Gagal..!";
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (_cache.Get<byte[]>(fileGuid) != null)
            {
                byte[] data = _cache.Get<byte[]>(fileGuid);
                _cache.Remove(fileGuid); //cleanup here as we don't need it in cache anymore
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                // Something has gone wrong...
                return View("Error"); // or whatever/wherever you want to return the user
            }
        }
    }
}
