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

        public async Task<AppLog> Insert(AppLog entity,string modul, string operasi)
        {
            entity.LgDate = DateTime.Now;
            entity.SysCode = "SPPB";
            if (operasi == "Tambah")
            {
                entity.LgModule = modul + "C";
                entity.LgOperation = "Tambah";
            }
            else if (operasi == "Hapus")
            {
                entity.LgModule = modul + "D";
                entity.LgOperation = "Hapus";
            }
            else if (operasi == "Ubah")
            {
                entity.LgModule = modul + "E";
                entity.LgOperation = "Ubah";
            }
            else if (operasi == "Posting")
            {
                entity.LgModule = modul + "T";
                entity.LgOperation = "Posting";
            }
            else if (operasi == "UnPosting")
            {
                entity.LgModule = modul + "UT";
                entity.LgOperation = "UnPosting";
            }
            else if (operasi == "Cetak")
            {
                entity.LgModule = modul + "P";
                entity.LgOperation = "Cetak";
            }
            else if (operasi == "Batal")
            {
                entity.LgModule = modul + "B";
                entity.LgOperation = "Batal";
            }
            else if (operasi == "Rollback")
            {
                entity.LgModule = modul + "R";
                entity.LgOperation = "Rollback";
            }
            else if (operasi == "Rekup")
            {
                entity.LgModule = modul + "T";
                entity.LgOperation = "Rekup";
            }
            await context.AppLog.AddAsync(entity);

            return entity;
        }
    }
}
