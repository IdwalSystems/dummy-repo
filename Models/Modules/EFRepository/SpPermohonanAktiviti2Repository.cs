using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class SpPermohonanAktiviti2Repository : ListViewIRepository<SpPermohonanAktiviti2, int>
    {
        public readonly ApplicationDbContext context;

        public SpPermohonanAktiviti2Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.SpPermohonanAktiviti2.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPermohonanAktiviti2>> GetAll(int sPPermohonanAktivitiId)
        {
            return await context.SpPermohonanAktiviti2
                //.Include(b => b.JCaraBayar)
                .Where(x => x.SpPermohonanAktivitiId == sPPermohonanAktivitiId)
                .ToListAsync();
        }

        public Task<SpPermohonanAktiviti2> GetBy2Id(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPermohonanAktiviti2> GetById(int id)
        {
            return await context.SpPermohonanAktiviti2.FindAsync(id);
        }

        public async Task<SpPermohonanAktiviti2> Insert(SpPermohonanAktiviti2 entity)
        {
            await context.SpPermohonanAktiviti2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPermohonanAktiviti2 entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
