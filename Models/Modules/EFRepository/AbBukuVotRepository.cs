using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AbBukuVotRepository : IRepository<AbBukuVot, int>
    {
        public readonly ApplicationDbContext context;

        public AbBukuVotRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AbBukuVot.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AbBukuVot>> GetAll()
        {
            return await context.AbBukuVot
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .ToListAsync();
        }

        public async Task<AbBukuVot> GetById(int id)
        {
            return await context.AbBukuVot.FindAsync(id);
        }

        public async Task<AbBukuVot> Insert(AbBukuVot entity)
        {
            await context.AbBukuVot.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AbBukuVot entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
