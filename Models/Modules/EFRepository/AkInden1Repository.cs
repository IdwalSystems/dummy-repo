using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkInden1Repository : ListViewIRepository<AkInden1, int>
    {
        public readonly ApplicationDbContext context;

        public AkInden1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkInden1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkInden1>> GetAll(int akIndenId)
        {
            return await context.AkInden1
                .Include(b => b.AkCarta)
                .Where(x => x.AkIndenId == akIndenId)
                .ToListAsync();
        }

        public async Task<AkInden1> GetBy2Id(int akIndenId, int akCartaId)
        {
            return await context.AkInden1.FirstOrDefaultAsync(x => x.AkIndenId == akIndenId && x.AkCartaId == akCartaId);
        }

        public async Task<AkInden1> GetById(int id)
        {
            return await context.AkInden1.Include(x => x.AkCarta)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkInden1> Insert(AkInden1 entity)
        {
            await context.AkInden1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInden1 entity)
        {
            AkInden1 data = context.AkInden1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            //Tambah kalau ada data dalam field lagi
            await context.SaveChangesAsync();
        }
    }
}
