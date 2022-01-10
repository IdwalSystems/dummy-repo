using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTerimaRepository : IRepository<AkTerima, int>
    {
        public readonly ApplicationDbContext context;

        public AkTerimaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTerima.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTerima>> GetAll()
        {
            return await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                .Include(b => b.AkTerima2)
                .ToListAsync();
        }

        public async Task<AkTerima> GetById(int id)
        {
            return await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1).ThenInclude(b=> b.AkCarta)
                .Include(b => b.AkTerima2).ThenInclude(b=> b.JCaraBayar)
                .Where(b=> b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkTerima> Insert(AkTerima entity)
        {
            await context.AkTerima.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTerima entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
