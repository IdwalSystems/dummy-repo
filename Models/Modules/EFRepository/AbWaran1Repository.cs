using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AbWaran1Repository : ListViewIRepository<AbWaran1, int>
    {
        public readonly ApplicationDbContext context;

        public AbWaran1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AbWaran.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AbWaran1>> GetAll(int abWaranId)
        {
            return await context.AbWaran1.Include(b => b.AkCarta)
                .Where(x => x.AbWaranId == abWaranId)
                .ToArrayAsync();
        }

        public async Task<AbWaran1> GetBy2Id(int abWaranId, int akCartaId)
        {
            return await context.AbWaran1
                .FirstOrDefaultAsync(x => x.AbWaranId == abWaranId && x.AkCartaId == akCartaId);
        }

        public async Task<AbWaran1> GetById(int id)
        {
            return await context.AbWaran1
                .Include(d => d.AkCarta)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AbWaran1> Insert(AbWaran1 entity)
        {
            await context.AbWaran1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AbWaran1 entity)
        {
            AbWaran1 data = context.AbWaran1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            data.TK = entity.TK;
            await context.SaveChangesAsync();
        }
    }
}
