using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkCartaRepository : IRepository<AkCarta, int>
    {
        public readonly ApplicationDbContext context;

        public AkCartaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var akCarta = await context.AkCarta.FirstOrDefaultAsync(b => b.id == id);
            if(akCarta != null)
            {
                context.Remove(akCarta);
            }
        }

        public async Task<IEnumerable<AkCarta>> GetAll()
        {
            return await context.AkCarta.Include(b => b.KW).Include(b =>b.Paras).Include(b => b.Jenis).ToListAsync();
        }

        public async Task<AkCarta> GetById(int id)
        {
            return await context.AkCarta.FindAsync(id);
        }

        public async Task<AkCarta> Insert(AkCarta entity)
        {
            await context.AkCarta.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkCarta entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();

        }
    }
}
