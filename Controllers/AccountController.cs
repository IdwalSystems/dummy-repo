using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Login.ViewModel;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        public AccountController(
            ApplicationDbContext db, 
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IRepository<SuPekerja, int, string> suPekerja)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _suPekerjaRepo = suPekerja;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(string returnurl=null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                //create role
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            var role = _roleManager.Roles.ToList();
            foreach(IdentityRole item in role)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = item.Name,
                    Text = item.Name
                });
            }

            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };

            ViewBag.SuPekerja = await _suPekerjaRepo.GetAll();

            return View(registerViewModel);
        }

        // redirect to login controller
        [HttpGet]
        public async Task<JsonResult> JsonLogOff()
        {
            try
            {
                await LogOff();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //redirect to login end

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl=null)
        {
            

            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if(model.SuPekerjaId != 0)
            {
                //check if user already exist in SuPekerja or not
                //if true then form is valid
                var pekerja = await _suPekerjaRepo.GetById((int)model.SuPekerjaId);
                if (pekerja != null)
                {
                    model.Nama = pekerja.Nama;

                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            Nama = pekerja.Nama,
                            SuPekerjaId = model.SuPekerjaId
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == "Admin")
                            {
                                await _userManager.AddToRoleAsync(user, "Admin");
                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, "User");
                            }
                            if (!User.IsInRole("Admin"))
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                return LocalRedirect(returnurl);
                            }
                            else
                            {
                                TempData[SD.Success] = "Data pengguna berjaya ditambah.";
                                return RedirectToAction(nameof(UserController.Index), "User");
                            }


                        }
                        AddErrors(result);

                    }
                }
                else
                {
                    TempData[SD.Error] = "Pengguna belum didaftar pada Jadual Anggota.";
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

            model.RoleList = listItems;

            ViewBag.SuPekerja = await _suPekerjaRepo.GetAll();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin , Supervisor , User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                    (
                        model.Emel,
                        model.Katalaluan,
                        model.IngatSaya,
                        lockoutOnFailure:true
                    );

                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }
                if (result.IsLockedOut) 
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cubaan log masuk tidak sah");
                    return View(model);
                }

            }
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty,error.Description);
            }
        }
    }
}
