using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkIndenLaras2Repository : ListViewIRepository<AkIndenLaras2, int>
    {
        public readonly ApplicationDbContext context;

        public AkIndenLaras2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkIndenLaras2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkIndenLaras2>> GetAll(int akIndenLarasId)
        {
            return await context.AkIndenLaras2
                .Where(x => x.AkIndenLarasId == akIndenLarasId)
                .ToListAsync();
        }

        public Task<AkIndenLaras2> GetBy2Id(int akIndenLarasId, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<AkIndenLaras2> GetById(int id)
        {
            return await context.AkIndenLaras2.FindAsync(id);
        }

        public async Task<AkIndenLaras2> Insert(AkIndenLaras2 entity)
        {
            await context.AkIndenLaras2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkIndenLaras2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
