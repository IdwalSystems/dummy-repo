using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class NegeriRepository : IRepository<Negeri, int>
    {
        public readonly ApplicationDbContext context;

        public NegeriRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.Negeri.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<Negeri>> GetAll()
        {
            return await context.Negeri.ToListAsync();
        }

        public async Task<Negeri> GetById(int id)
        {
            return await context.Negeri.FindAsync(id);

        }

        public async Task<Negeri> Insert(Negeri entity)
        {
            await context.Negeri.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(Negeri entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
