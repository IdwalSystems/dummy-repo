using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkNotaDebitKreditBelian2Repository : ListViewIRepository<AkNotaDebitKreditBelian2, int>
    {
        public readonly ApplicationDbContext context;
        public AkNotaDebitKreditBelian2Repository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var model = await context.AkNotaDebitKreditBelian2.FirstOrDefaultAsync(b => b.Id == id);
            if(model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian2>> GetAll(int akNotaDebitKreditBelianId)
        {
            return await context.AkNotaDebitKreditBelian2
                .Where(x => x.AkNotaDebitKreditBelianId == akNotaDebitKreditBelianId)
                .ToArrayAsync();
        }

        public async Task<AkNotaDebitKreditBelian2> GetBy2Id(int akNotaDebitKreditBelianId, int indek)
        {
            return await context.AkNotaDebitKreditBelian2.FirstOrDefaultAsync(x => x.AkNotaDebitKreditBelianId == akNotaDebitKreditBelianId && x.Indek == indek);
        }

        public async Task<AkNotaDebitKreditBelian2> GetById(int id)
        {
            return await context.AkNotaDebitKreditBelian2.FindAsync(id);
        }

        public async Task<AkNotaDebitKreditBelian2> Insert(AkNotaDebitKreditBelian2 entity)
        {
            await context.AkNotaDebitKreditBelian2.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaDebitKreditBelian2 entity)
        {
            AkNotaDebitKreditBelian2 data = context.AkNotaDebitKreditBelian2.FirstOrDefault(x => x.Id == entity.Id);

            data.Bil = entity.Bil;
            data.NoStok = entity.NoStok;
            data.Perihal = entity.Perihal;
            data.Kuantiti = entity.Kuantiti;
            data.Unit = entity.Unit;
            data.Harga = entity.Harga;
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
