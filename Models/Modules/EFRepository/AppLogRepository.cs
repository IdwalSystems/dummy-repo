using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    
    public class AppLogRepository : AppLogIRepository<AppLog, int>
    {
        public ClaimsPrincipal User { get; private set; }

        public readonly ApplicationDbContext context;
        public readonly UserManager<IdentityUser> userManager;
        public AppLogRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<AppLog>> GetAll()
        {
            return await context.AppLog.ToListAsync();
        }

        public async Task<AppLog> Insert(AppLog entity)
        {
            entity.LgDate = DateTime.Now;
            entity.SysCode = "SPPB";
            await context.AppLog.AddAsync(entity);
            return entity;
        }
    }
}
