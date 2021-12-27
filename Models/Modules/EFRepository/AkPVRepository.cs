using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPVRepository : IRepository<AkPV, int>
    {
        public readonly ApplicationDbContext context;

        public AkPVRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPV.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPV>> GetAll()
        {
            return await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                .Include(b => b.AkPV2)
                .ThenInclude(b=> b.AkBelian)
                .ToListAsync();
        }

        public async Task<AkPV> GetById(int id)
        {
            return await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                .ThenInclude(b=>b.AkCarta)
                .Include(b => b.AkPV2)
                .ThenInclude(b => b.AkBelian)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPV> Insert(AkPV entity)
        {
            await context.AkPV.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPV entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
