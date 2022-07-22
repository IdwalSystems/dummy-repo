using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AppLogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AppLogController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AppLog
        public async Task<IActionResult> Index(
            string searchUsername,
            string searchModulFrom,
            string searchModulTo,
            string searchDateFrom,
            string searchDateTo)
        {
            var appLog = new List<AppLog>();

            if (!String.IsNullOrEmpty(searchUsername) ||
                !String.IsNullOrEmpty(searchModulFrom) ||
                !String.IsNullOrEmpty(searchModulTo) ||
                !String.IsNullOrEmpty(searchDateFrom) ||
                !String.IsNullOrEmpty(searchDateTo))
            {
                appLog = _context.AppLog.ToList();
            }

            //function search userList
            var userList = await _context.applicationUsers.Include(x=> x.SuPekerja).ToListAsync();

            List<SelectListItem> userSelect = new();
            userSelect.Add(new SelectListItem() { Text = "-- Pilih Pengguna --", Value = "" });
            foreach (var q in userList)
            {
                userSelect.Add(new SelectListItem() { Text = q.Nama + " (" + q.UserName + ")", Value = q.UserName });
            }

            if (!String.IsNullOrEmpty(searchUsername))
            {
                ViewBag.username = new SelectList(userSelect, "Value", "Text", searchUsername);
                appLog = appLog.Where(x => x.UserId == searchUsername).ToList();
            }
            else
            {
                ViewBag.username = new SelectList(userSelect, "Value", "Text", "");
            }
            //function search userList end

            //function search date
            if (!String.IsNullOrEmpty(searchDateFrom) && !String.IsNullOrEmpty(searchDateTo))
            {
                DateTime date1 = DateTime.Parse(searchDateFrom);
                DateTime date2 = DateTime.Parse(searchDateTo);

                appLog = appLog.Where(x => x.LgDate >= date1 && x.LgDate <= date2).ToList();

                ViewData["DateFrom"] = searchDateFrom;
                ViewData["DateTo"] = searchDateTo;
                //akAkaun = akAkaun.OrderByDescending(c => c.Tarikh.Date).ThenBy(c => c.Tarikh.TimeOfDay);

            }
            //function search date end

            //function search modul range
            if (!String.IsNullOrEmpty(searchModulFrom) && !String.IsNullOrEmpty(searchModulTo))
            {
                Tuple<string, string> range = Tuple.Create(searchModulFrom, searchModulTo);
                appLog = appLog.Where(s =>
                        range.Item1.CompareTo(s.LgModule.Substring(0, range.Item1.Length)) <= 0 &&
                        s.LgModule.Substring(0, range.Item2.Length).CompareTo(range.Item2) <= 0)
                        .ToList();

                ViewData["ModulFrom"] = searchModulFrom;
                ViewData["ModulTo"] = searchModulTo;
            }
            // function search modul range end
            return View(appLog.OrderBy(b => b.LgDate).ThenBy(b => b.LgModule).ToList());
        }
   
    }
}
