using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MSNK.Models.Modules.ViewModel.UserClaimsViewModel;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : Controller
    {
        public const string modul = "SY001";
        public const string namamodul = "Sistem Pengguna";

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public UserController(
            ApplicationDbContext db, 
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog,int> appLog)
        {
            _db = db;
            _userManager = userManager;
            _appLog = appLog;
        }
        private async Task AddLogAsync(
            string operasi,
            string nota,
            string rujukan,
            int idRujukan,
            decimal jumlah)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;

            await _appLog.Insert(appLog, modul, operasi);
        }

        public IActionResult Index()
        {
            var userList = _db.applicationUsers.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in userList)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    if(user.Role == "SuperAdmin")
                    {
                        continue;
                    }
                    else
                    {
                        user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                    }
                }
            }
            //hide superadmin
                //userList = userList.Where(x => x.Role != "SuperAdmin").ToList();

            return View(userList);
        }
        
        public IActionResult Register()
        {
            return RedirectToAction(nameof(AccountController.Register), "Account");
        }

        public IActionResult Edit(string userId)
        {
            var objFromDb = _db.applicationUsers.FirstOrDefault(u=>u.Id == userId);
            if(objFromDb == null)
            {
                return NotFound();
            }
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            var role = userRole.FirstOrDefault(u => u.UserId == userId);
            if (role != null)
            {
                objFromDb.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
            }
            objFromDb.RoleList = _db.Roles.Where(x => x.Name != "SuperAdmin").Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            });

            //List<SelectListItem> listItems = new List<SelectListItem>();
            //listItems.Add(new SelectListItem()
            //{
            //    Value = "Admin",
            //    Text = "Admin"
            //});
            //listItems.Add(new SelectListItem()
            //{
            //    Value = "Supervisor",
            //    Text = "Supervisor"
            //});
            //listItems.Add(new SelectListItem()
            //{
            //    Value = "User",
            //    Text = "User"
            //});

            //objFromDb.RoleList = listItems;

            return View(objFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if(user.RoleId != null)
            {
                if (ModelState.IsValid)
                {
                    var objFromDb = _db.applicationUsers.FirstOrDefault(u => u.Id == user.Id);
                    var roleAsal = "";
                    var roleBaru = "";
                    if (objFromDb == null)
                    {
                        return NotFound();
                    }
                    var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == objFromDb.Id);

                    if (userRole != null)
                    {
                        var previousRoleName = _db.Roles.Where(u => u.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                        var newRoleName = _db.Roles.Where(u => u.Id == user.RoleId).Select(e => e.Name).FirstOrDefault();
                        roleAsal = previousRoleName;
                        roleBaru = newRoleName;
                        //removing old role
                        await _userManager.RemoveFromRoleAsync(objFromDb, previousRoleName);

                        //add new role
                        await _userManager.AddToRoleAsync(objFromDb, _db.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);
                    }
                    else
                    {
                        //add new role
                        await _userManager.AddToRoleAsync(objFromDb, user.RoleId);
                        
                        
                    }
                    objFromDb.Nama = user.Nama;
                    if (roleAsal != roleBaru)
                    {
                        await AddLogAsync("Ubah", roleAsal + " -> " + roleBaru, user.Email, 0, 0);

                    }
                    _db.SaveChanges();
                    TempData[SD.Success] = "Data pengguna berjaya diubah.";
                    return RedirectToAction(nameof(Index));
                }
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "Supervisor",
                Text = "Supervisor"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });

            user.RoleList = listItems;

            //user.RoleList = _db.Roles.Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id
            //});
            TempData[SD.Error] = "Sila Pilih Tahap Pengguna..!";
            return View(user);
        }
        [HttpPost]
        public IActionResult LockUnlock(string userId)
        {
            var objFromDb = _db.applicationUsers.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd>DateTime.Now)
            {
                // user is locked and will remain locked until lockoutend time
                //clicking on this action will unlock time
                objFromDb.LockoutEnd = DateTime.Now;
                TempData[SD.Success] = "Buka kunci pengguna berjaya.";

            }
            else
            {
                //user is not locked, and we want to lock user
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                TempData[SD.Success] = "Kunci pengguna berjaya.";
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var objFromDb = _db.applicationUsers.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            _db.applicationUsers.Remove(objFromDb);
            await AddLogAsync("Hapus", objFromDb.Email + " - " + objFromDb.Nama, objFromDb.Email, 0, 0);

            _db.SaveChanges();
            TempData[SD.Success] = "Hapus pengguna berjaya.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId
            };

            foreach(Claim claim in ClaimStore.claimsList)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };
                if(existingUserClaims.Any(c=>c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel userClaimsViewModel)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user,claims);

            if(!result.Succeeded)
            {
                TempData[SD.Error] = "Ralat ketika menghapuskan capaian.";
            }

            result = await _userManager.AddClaimsAsync(user,
                userClaimsViewModel.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString())));

            if (!result.Succeeded)
            {
                TempData[SD.Error] = "Ralat ketika menambah capaian.";
            }
            await AddLogAsync("Ubah", user.Email + " - Ubah Capaian", user.Email, 0, 0);

            _db.SaveChanges();
            TempData[SD.Success] = "Capaian berjaya diubah.";
            return RedirectToAction(nameof(Index));
        }
    }
}
