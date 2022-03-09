using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Login.ViewModel;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public ProfileController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _db = db;
            _userManager = userManager;
            _appLog = appLog;
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _db.applicationUsers.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            ResetPasswordViewModel viewModel = new ResetPasswordViewModel();

            viewModel.Email = user.Email;

            return View(viewModel);
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
    }
}
