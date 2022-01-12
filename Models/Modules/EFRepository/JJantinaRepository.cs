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
    public class JJantinaRepository : IRepository<JJantina, int>
    {
        private readonly ApplicationDbContext context;
        public JJantinaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var jantina = await context.JJantina.FirstOrDefaultAsync(b => b.Id == id);
            if (jantina != null)
            {
                context.Remove(jantina);
            }
        }

        public async Task<IEnumerable<JJantina>> GetAll()
        {
            return await context.JJantina.ToListAsync();
        }

        public async Task<JJantina> GetById(int id)
        {
            return await context.JJantina.FindAsync(id);
        }

        public async Task<JJantina> Insert(JJantina entity)
        {
            await context.JJantina.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JJantina entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
