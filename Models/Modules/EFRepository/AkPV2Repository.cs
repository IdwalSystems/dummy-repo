using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPV2Repository : ListViewIRepository<AkPV2, int>
    {
        public readonly ApplicationDbContext context;

        public AkPV2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPV2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPV2>> GetAll(int akPVId)
        {
            return await context.AkPV2
                .Include(b => b.AkBelian).ThenInclude(b=> b.AkPO)
                .Where(x => x.AkPVId == akPVId)
                .ToListAsync();
        }

        public async Task<AkPV2> GetBy2Id(int akPVId, int akBelianId)
        {
            return await context.AkPV2.FirstOrDefaultAsync(x => x.AkPVId == akPVId && x.AkBelianId == akBelianId);
        }

        public async Task<AkPV2> GetById(int id)
        {
            return await context.AkPV2
                .Include(b => b.AkBelian).ThenInclude(b=>b.AkPO)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPV2> Insert(AkPV2 entity)
        {
            await context.AkPV2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPV2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
