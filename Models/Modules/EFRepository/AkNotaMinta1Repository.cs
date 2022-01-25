using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkNotaMinta1Repository : ListViewIRepository<AkNotaMinta1, int>
    {
        public readonly ApplicationDbContext context;

        public AkNotaMinta1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkNotaMinta1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaMinta1>> GetAll(int AkNotaMintaId)
        {
            return await context.AkNotaMinta1.Include(b => b.AkCarta)
                .Where(x => x.AkNotaMintaId == AkNotaMintaId)
                .ToArrayAsync();
        }

        public async Task<AkNotaMinta1> GetBy2Id(int akNotaMintaId, int akCartaId)
        {
            return await context.AkNotaMinta1.FirstOrDefaultAsync(x => x.AkNotaMintaId == akNotaMintaId && x.AkCartaId == akCartaId);
        }

        public async Task<AkNotaMinta1> GetById(int id)
        {
            return await context.AkNotaMinta1.Include(d => d.AkCarta).Where(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkNotaMinta1> Insert(AkNotaMinta1 entity)
        {
            await context.AkNotaMinta1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaMinta1 entity)
        {
            AkNotaMinta1 data = context.AkNotaMinta1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
