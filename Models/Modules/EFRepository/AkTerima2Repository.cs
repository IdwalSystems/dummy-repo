using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTerima2Repository : ListViewIRepository<AkTerima2, int>
    {
        public readonly ApplicationDbContext context;

        public AkTerima2Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTerima2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTerima2>> GetAll(int akTerimaId)
        {
            return await context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(x=> x.AkTerimaId == akTerimaId)
                .ToListAsync();
        }

        public async Task<AkTerima2> GetBy2Id(int akTerimaId, int jCaraBayarId)
        {
            return await context.AkTerima2.FirstOrDefaultAsync(x => x.AkTerimaId == akTerimaId && x.JCaraBayarId == jCaraBayarId);
        }

        public async Task<AkTerima2> GetById(int id)
        {
            return await context.AkTerima2.FindAsync(id);
        }

        public async Task<AkTerima2> Insert(AkTerima2 entity)
        {
            await context.AkTerima2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTerima2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
