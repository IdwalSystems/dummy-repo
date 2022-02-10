using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPOLaras1Repository : ListViewIRepository<AkPOLaras1, int>
    {
        public readonly ApplicationDbContext context;

        public AkPOLaras1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPOLaras1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPOLaras1>> GetAll(int akPOLarasId)
        {
            return await context.AkPOLaras1
                .Include(b => b.AkCarta)
                .Where(x => x.AkPOLarasId == akPOLarasId)
                .ToListAsync();
        }

        public async Task<AkPOLaras1> GetBy2Id(int akPOLarasId, int akCartaId)
        {
            return await context.AkPOLaras1.FirstOrDefaultAsync(x => x.AkPOLarasId == akPOLarasId && x.AkCartaId == akCartaId);
        }

        public async Task<AkPOLaras1> GetById(int id)
        {
            return await context.AkPOLaras1.Include(x => x.AkCarta)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkPOLaras1> Insert(AkPOLaras1 entity)
        {
            await context.AkPOLaras1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPOLaras1 entity)
        {

            AkPOLaras1 data = context.AkPOLaras1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
