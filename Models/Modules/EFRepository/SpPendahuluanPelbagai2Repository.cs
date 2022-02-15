using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class SpPendahuluanPelbagai2Repository : ListViewIRepository<SpPendahuluanPelbagai2, int>
    {
        public readonly ApplicationDbContext context;

        public SpPendahuluanPelbagai2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.SpPendahuluanPelbagai2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai2>> GetAll(int sPPermohonanAktivitiId)
        {
            return await context.SpPendahuluanPelbagai2
                //.Include(b => b.JJantina)
                .Where(x => x.SpPendahuluanPelbagaiId == sPPermohonanAktivitiId)
                .ToListAsync();
        }

        public Task<SpPendahuluanPelbagai2> GetBy2Id(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPendahuluanPelbagai2> GetById(int id)
        {
            return await context.SpPendahuluanPelbagai2.FindAsync(id);
        }

        public async Task<SpPendahuluanPelbagai2> Insert(SpPendahuluanPelbagai2 entity)
        {
            await context.SpPendahuluanPelbagai2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPendahuluanPelbagai2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
