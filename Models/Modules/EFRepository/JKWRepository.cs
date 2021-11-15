using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    [Authorize]
    public class JKWRepository : IRepository<JKW, int>
    {
        private readonly ApplicationDbContext context;
        public JKWRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var kw = await context.JKW.FirstOrDefaultAsync(b => b.Id == id);
            if (kw != null)
            {
                context.Remove(kw);
            }
        }

        public async Task<IEnumerable<JKW>> GetAll()
        {
            return await context.JKW.ToListAsync();
        }

        public async Task<JKW> GetById(int id)
        {
            return await context.JKW.FindAsync(id);
        }

        public async Task<JKW> Insert(JKW entity)
        {
            await context.JKW.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JKW entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
