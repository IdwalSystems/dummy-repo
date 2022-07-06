using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkBelian2Repository : ListViewIRepository<AkBelian2, int>
    {
        public readonly ApplicationDbContext context;

        public AkBelian2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBelian2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkBelian2>> GetAll(int akBelianId)
        {
            return await context.AkBelian2
                .Where(x => x.AkBelianId == akBelianId)
                .ToArrayAsync();
        }

        public async Task<AkBelian2> GetBy2Id(int akBelianId, int indek)
        {
            return await context.AkBelian2.FirstOrDefaultAsync(x => x.AkBelianId == akBelianId && x.Indek == indek);
        }

        public async Task<AkBelian2> GetById(int id)
        {
            return await context.AkBelian2.FindAsync(id);
        }

        public async Task<AkBelian2> Insert(AkBelian2 entity)
        {
            await context.AkBelian2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBelian2 entity)
        {
            AkBelian2 data = context.AkBelian2.FirstOrDefault(x => x.Id == entity.Id);

            data.Bil = entity.Bil;
            data.NoStok = entity.NoStok;
            data.Perihal = entity.Perihal;
            data.Kuantiti = entity.Kuantiti;
            data.Unit = entity.Unit;
            data.Harga = entity.Harga;
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
