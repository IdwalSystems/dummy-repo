using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class AkPO2Repository : IRepository<AkPO2, int>
    {
        public readonly ApplicationDbContext context;

        public AkPO2Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkPO2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPO2>> GetAll()
        {
            return await context.AkPO2
                .ToListAsync();
        }

        public async Task<AkPO2> GetById(int id)
        {
            return await context.AkPO2.FindAsync(id);
        }

        public async Task<AkPO2> Insert(AkPO2 entity)
        {
            await context.AkPO2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        Task<AkPO2> IRepository<AkPO2, int>.GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
