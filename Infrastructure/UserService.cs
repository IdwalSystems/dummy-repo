using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
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

        public async Task<CompanyDetails> GetCompanyDetails()
        {
            CompanyDetails company = new CompanyDetails();

            SiAppInfo appInfo = await _db.SiAppInfo.FirstOrDefaultAsync();

            if(appInfo != null)
            {

                company.KodSistem = appInfo.KodSistem;
                company.NamaSyarikat = appInfo.NamaSyarikat;
                company.NoPendaftaran = appInfo.NoPendaftaran;
                company.AlamatSyarikat1 = appInfo.AlamatSyarikat1;
                company.AlamatSyarikat2 = appInfo.AlamatSyarikat2;
                company.AlamatSyarikat3 = appInfo.AlamatSyarikat3;
                company.Bandar = appInfo.Bandar;
                company.Poskod = appInfo.Poskod;
                company.Daerah = appInfo.Daerah;
                company.Negeri = appInfo.Negeri;
                company.TelSyarikat = appInfo.TelSyarikat;
                company.FaksSyarikat = appInfo.FaksSyarikat;
                company.EmelSyarikat = appInfo.EmelSyarikat;
                company.CompanyLogoWeb = "MainLogo_Syarikat.webp";
                company.CompanyLogoPrintPDF = "MainLogo_Syarikat.png";
                company.TarMula = appInfo.TarMula;
            };

            return company; 
        }
        public async Task Impersonate(string userId)
        {

            ApplicationUser user = (ApplicationUser)await _userManager.FindByIdAsync(userId);
            await _signInManager.SignInAsync(user, false);
        }

        public bool IsUnderMaintainance()
        {
            SiAppInfo appInfo = _db.SiAppInfo.FirstOrDefault();

            if(appInfo != null)
            {
                if(appInfo.FlMaintainance == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
