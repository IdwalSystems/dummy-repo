using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class AkPO1Repository : AkPO1IRepository<AkPO1, int>
    {
        public readonly ApplicationDbContext context;

        public AkPO1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPO1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPO1>> GetAll(int akPOId)
        {
            return await context.AkPO1
                .Include(b => b.AkCarta)
                .ToListAsync();
        }

        public async Task<AkPO1> GetBy2Id(int akPOId, int akCartaId)
        {
            return await context.AkPO1.FirstOrDefaultAsync(x => x.AkPOId == akPOId && x.AkCartaId == akCartaId);
        }

        public async Task<AkPO1> GetById(int id)
        {
            return await context.AkPO1.FindAsync(id);
        }

        public async Task<AkPO1> Insert(AkPO1 entity)
        {
            await context.AkPO1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO1 entity)
        {

            AkPO1 data = context.AkPO1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
