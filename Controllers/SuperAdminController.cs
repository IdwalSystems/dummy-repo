using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSNK.Data;
using System.Linq;

namespace MSNK.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuperAdminController(ApplicationDbContext context)
        {
            _context=context;
        }
        public IActionResult ExceptionLog()
        {
            var el = _context.ExceptionLogger.OrderByDescending(l => l.LogTime).ToList();
            return View(el);
        }
    }
}
