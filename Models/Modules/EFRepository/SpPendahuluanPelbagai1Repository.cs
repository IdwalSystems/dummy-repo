using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class SpPendahuluanPelbagai1Repository : ListViewIRepository<SpPendahuluanPelbagai1, int>
    {
        public readonly ApplicationDbContext context;

        public SpPendahuluanPelbagai1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.SpPendahuluanPelbagai1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai1>> GetAll(int sPPermohonanAktivitiId)
        {
            return await context.SpPendahuluanPelbagai1
                //.Include(b => b.AkCarta)
                .ToListAsync();
        }

        public Task<SpPendahuluanPelbagai1> GetBy2Id(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPendahuluanPelbagai1> GetById(int id)
        {
            return await context.SpPendahuluanPelbagai1.FindAsync(id);
        }

        public async Task<SpPendahuluanPelbagai1> Insert(SpPendahuluanPelbagai1 entity)
        {
            await context.SpPendahuluanPelbagai1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPendahuluanPelbagai1 entity)
        {

            SpPendahuluanPelbagai1 data = context.SpPendahuluanPelbagai1.FirstOrDefault(x => x.Id == entity.Id);
            data.JumL = entity.JumL;
            data.JumP = entity.JumP;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
