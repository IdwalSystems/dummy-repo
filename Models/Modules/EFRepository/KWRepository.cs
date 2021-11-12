using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class KWRepository : IRepository<KW, int>
    {
        private readonly ApplicationDbContext context;
        public KWRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var kw = await context.KW.FirstOrDefaultAsync(b => b.Id == id);
            if (kw != null)
            {
                context.Remove(kw);
            }
        }

        public async Task<IEnumerable<KW>> GetAll()
        {
            return await context.KW.ToListAsync();
        }

        public async Task<KW> GetById(int id)
        {
            return await context.KW.FindAsync(id);
        }

        public async Task<KW> Insert(KW entity)
        {
            await context.KW.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(KW entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
