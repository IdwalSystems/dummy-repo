using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    
    public class AkPORepository : IRepository<AkPO, int>
    {
        public readonly ApplicationDbContext context;

        public AkPORepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkPO.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPO>> GetAll()
        {
            return await context.AkPO
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
        }

        public async Task<AkPO> GetById(int id)
        {
            return await context.AkPO
                .Where(d => d.Id == id)
                .Include(d=>d.AkPO1).ThenInclude(d=>d.AkCarta)
                .Include(d=>d.AkPO2)
                .Include(d=> d.AkPembekal)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPO> Insert(AkPO entity)
        {
            await context.AkPO.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
