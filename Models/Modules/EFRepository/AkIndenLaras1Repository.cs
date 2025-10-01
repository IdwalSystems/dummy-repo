using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkIndenLaras1Repository : ListViewIRepository<AkIndenLaras1, int>
    {
        public readonly ApplicationDbContext context;

        public AkIndenLaras1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkIndenLaras1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkIndenLaras1>> GetAll(int akIndenLarasId)
        {
            return await context.AkIndenLaras1
                .Include(b => b.AkCarta)
                .Where(x => x.AkIndenLarasId == akIndenLarasId)
                .ToListAsync();
        }

        public async Task<AkIndenLaras1> GetBy2Id(int akIndenLarasId, int akCartaId)
        {
            return await context.AkIndenLaras1.FirstOrDefaultAsync(x => x.AkIndenLarasId == akIndenLarasId && x.AkCartaId == akCartaId);
        }

        public async Task<AkIndenLaras1> GetById(int id)
        {
            return await context.AkIndenLaras1.Include(x => x.AkCarta)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkIndenLaras1> Insert(AkIndenLaras1 entity)
        {
            await context.AkIndenLaras1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkIndenLaras1 entity)
        {

            AkIndenLaras1 data = context.AkIndenLaras1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
