using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPVGandaRepository : ListViewIRepository<AkPVGanda, int>
    {
        public readonly ApplicationDbContext context;
        public AkPVGandaRepository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var model = await context.AkPVGanda.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPVGanda>> GetAll(int akPVId)
        {
            return await context.AkPVGanda
                .Include(b => b.SuPekerja)
                .Include(b => b.SuAtlet)
                .Include(b => b.SuJurulatih)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JBank)
                .Where(b => b.AkPVId == akPVId)
                .ToListAsync();
        }

        public async Task<AkPVGanda> GetBy2Id(int akPVId, int indek)
        {
            return await context.AkPVGanda
                .Include(b => b.AkPadananPenyata)
                .Where(b => b.AkPVId == akPVId && b.Indek == indek)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPVGanda> GetById(int id)
        {
            return await context.AkPVGanda
                .Include(b=> b.AkPadananPenyata)
                .Where( b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkPVGanda> Insert(AkPVGanda entity)
        {
            await context.AkPVGanda.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPVGanda entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
