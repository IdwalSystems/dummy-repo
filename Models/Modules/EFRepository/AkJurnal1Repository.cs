using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkJurnal1Repository : IRepository2<AkJurnal1, int>
    {
        public readonly ApplicationDbContext context;
        public AkJurnal1Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkJurnal1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkJurnal1>> GetAll(int id)
        {
            return await context.AkJurnal1
                .Include(b => b.AkCartaId)
                .Where(x => x.AkJurnalId == id)
                .ToListAsync();
        }

        public async Task<AkJurnal1> GetBy2Id(int id1, int id2)
        {
            return await context.AkJurnal1.FirstOrDefaultAsync(x => x.AkJurnalId == id1 && x.AkCartaId == id2);
        }

        public async Task<AkJurnal1> GetById(int id)
        {
            return await context.AkJurnal1.FindAsync(id);
        }

        public async Task<AkJurnal1> Insert(AkJurnal1 entity)
        {
            await context.AkJurnal1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkJurnal1 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
