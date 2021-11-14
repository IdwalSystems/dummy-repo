using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTerima1Repository : IRepository<AkTerima1, int>
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

        public async Task<IEnumerable<AkTerima1>> GetAll()
        {
            return await context.AkTerima1
                .Include(b => b.AkTerima)
                .Include(b => b.AkAkaun)
                .ToListAsync();
        }

        public async Task<AkTerima1> GetById(int id)
        {
            return await context.AkTerima1.FindAsync(id);
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
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
