using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Login.ViewModel;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    public class AccountController : Controller
    {
        public const string modul = "SY001";
        public const string namamodul = "Sistem Pengguna";

        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly SendGridEmailServices _mailServices; 
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        public AccountController(
            ApplicationDbContext db,
            IConfiguration configuration,
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            //IEmailSender emailSender,
            SendGridEmailServices mailServices,
            RoleManager<IdentityRole> roleManager,
            IRepository<SuPekerja, int, string> suPekerja,
            AppLogIRepository<AppLog,int> appLog)
        {
            _db = db;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailServices = mailServices;
            //_emailSender = emailSender;
            _roleManager = roleManager;
            _suPekerjaRepo = suPekerja;
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

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Register(string returnurl=null)
        {
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                //create role
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Supervisor"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            //List<SelectListItem> listItems = new List<SelectListItem>();
            //var role = _roleManager.Roles.ToList();
            //foreach(IdentityRole item in role)
            //{
            //    listItems.Add(new SelectListItem()
            //    {
            //        Value = item.Name,
            //        Text = item.Name
            //    });
            //}
            List<SelectListItem> listItems = new List<SelectListItem>();
            //listItems.Add(new SelectListItem()
            //{
            //    Value = "Admin",
            //    Text = "Admin"
            //});
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });

            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };

            ViewBag.SuPekerja = await _suPekerjaRepo.GetAll();

            return View(registerViewModel);
        }

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetEmailFromSuPekerja(int data)
        {
            try
            {
                var result = await _suPekerjaRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

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
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl=null)
        {
            

            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if(model.SuPekerjaId != 0)
            {
                //check if user already exist in SuPekerja or not
                //if true then form is valid
                var pekerja = await _suPekerjaRepo.GetById((int)model.SuPekerjaId);
                if (pekerja != null && pekerja.Emel == model.Email)
                {
                    model.Nama = pekerja.Nama;
                    model.Password = "Spmb1234#";

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
                            //if (!User.IsInRole("Admin"))
                            //{
                            //    await _signInManager.SignInAsync(user, isPersistent: false);
                            //    return LocalRedirect(returnurl);
                            //}
                            //else
                            //{
                                TempData[SD.Success] = "Data pengguna berjaya ditambah.";
                                await AddLogAsync("Tambah", model.Email + " - " + pekerja.Nama, model.Email, 0, 0);
                                _db.SaveChanges();
                                return RedirectToAction(nameof(UserController.Index), "User");
                            //}


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
            //listItems.Add(new SelectListItem()
            //{
            //    Value = "Admin",
            //    Text = "Admin"
            //});
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
        [Authorize]
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Emel);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordError");
                }
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                await SendMail(model);

                //await _emailSender.SendEmailAsync(model.Emel, "Set Semula Katalaluan - Identity Manager",
                //    "Sila set semula katalaluan anda dengan melayari pautan ini: <a href=\"" + callbackUrl + "\">link</a>");
                //await _mailServices.SendEmailAsync(model.Emel, "Set Semula Katalaluan Sistem SPMB",
                //    "Sila set semula katalaluan anda dengan melayari pautan ini:<br> <a href=\"" + callbackUrl + "\">"+callbackUrl+"</a>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        public async Task<int> SendMail(ForgotPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Emel);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            var profileName = _configuration["ProfileName"];

            var html = "<h4>Set Semula Katalaluan</h4>" +
                        "</ br>" +
                        "<p>Sila set semula katalaluan anda dengan melayari pautan ini:</p>" +
                        "<a href=" + callbackUrl + ">" + callbackUrl + "</a>";
            try
            {
                var query = "EXEC msdb.dbo.sp_send_dbmail " +
                            "@profile_name = '" + profileName + "', " +
                            "@recipients = '" + model.Emel + "', " +
                            "@body = '" + html + "', " +
                            "@body_format = 'HTML'," +
                            "@subject = 'Set Semula Katalaluan - Mesej Automatik'; ";

                var parameters = new DynamicParameters();
                parameters.Add("ProfileName", profileName, DbType.String);
                parameters.Add("Email", model.Emel, DbType.String);
                parameters.Add("CallbackUrl", callbackUrl, DbType.String);

                using (var connection = CreateConnection())
                {
                    return await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet]
        public IActionResult ForgotPasswordError()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code=null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                AddErrors(result);

            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
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
