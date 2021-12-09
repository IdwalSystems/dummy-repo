using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class AkPO1Repository : IRepository<AkPO1, int>
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

        public async Task<IEnumerable<AkPO1>> GetAll()
        {
            return await context.AkPO1
                .Include(b => b.JKW)
                .ToListAsync();
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
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        Task<AkPO1> IRepository<AkPO1, int>.GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
