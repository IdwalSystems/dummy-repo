using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTerima1Repository : ListViewIRepository<AkTerima1, int>
    {
        public readonly ApplicationDbContext context;

        public AkTerima1Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTerima1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTerima1>> GetAll(int akTerimaId)
        {
            return await context.AkTerima1.Include(b=> b.AkCarta)
                .Where(x=> x.AkTerimaId == akTerimaId)
                .ToArrayAsync();
        }

        public async Task<AkTerima1> GetBy2Id(int akTerimaId, int akCartaId)
        {
            return await context.AkTerima1.FirstOrDefaultAsync(x=> x.AkTerimaId == akTerimaId && x.AkCartaId == akCartaId);
        }

        public async Task<AkTerima1> GetById(int id)
        {
            return await context.AkTerima1.Include(d=> d.AkCarta).Where(x=> x.Id == id).FirstOrDefaultAsync();
        }


        public async Task<AkTerima1> Insert(AkTerima1 entity)
        {
            await context.AkTerima1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTerima1 entity)
        {

            AkTerima1 data = context.AkTerima1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
