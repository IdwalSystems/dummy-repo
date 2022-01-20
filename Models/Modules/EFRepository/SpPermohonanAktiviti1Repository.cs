using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{

    public class SpPermohonanAktiviti1Repository : ListViewIRepository<SpPermohonanAktiviti1, int>
    {
        public readonly ApplicationDbContext context;

        public SpPermohonanAktiviti1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.SpPermohonanAktiviti1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPermohonanAktiviti1>> GetAll(int sPPermohonanAktivitiId)
        {
            return await context.SpPermohonanAktiviti1
                .Include(b => b.AkCarta)
                .ToListAsync();
        }

        public async Task<SpPermohonanAktiviti1> GetBy2Id(int sPPermohonanAktivitiId, int akCartaId)
        {
            return await context.SpPermohonanAktiviti1.FirstOrDefaultAsync(x => x.SpPermohonanAktivitiId == sPPermohonanAktivitiId && x.AkCartaId == akCartaId);
        }

        public async Task<SpPermohonanAktiviti1> GetById(int id)
        {
            return await context.SpPermohonanAktiviti1.FindAsync(id);
        }

        public async Task<SpPermohonanAktiviti1> Insert(SpPermohonanAktiviti1 entity)
        {
            await context.SpPermohonanAktiviti1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPermohonanAktiviti1 entity)
        {

            SpPermohonanAktiviti1 data = context.SpPermohonanAktiviti1.FirstOrDefault(x => x.Id == entity.Id);
            data.Jumlah = entity.Jumlah;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
