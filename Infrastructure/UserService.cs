using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin;
using MSNK.Data;
using MSNK.Models.Administration;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Infrastructure
{
    public class UserService
    {
        private ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Impersonate(string userId)
        {

            ApplicationUser user = (ApplicationUser)await _userManager.FindByIdAsync(userId);
            await _signInManager.SignInAsync(user, false);
        }
    }
}
