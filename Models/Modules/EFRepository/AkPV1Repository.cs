using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPV1Repository : ListViewIRepository<AkPV1, int>
    {
        public readonly ApplicationDbContext context;

        public AkPV1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPV1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPV1>> GetAll(int akPVId)
        {
            return await context.AkPV1.Include(b => b.AkCarta)
                .Where(x => x.AkPVId == akPVId)
                .ToArrayAsync();
        }

        public async Task<AkPV1> GetBy2Id(int akPVId, int akCartaId)
        {
            return await context.AkPV1.FirstOrDefaultAsync(x => x.AkPVId == akPVId && x.AkCartaId == akCartaId);
        }

        public async Task<AkPV1> GetById(int id)
        {
            return await context.AkPV1
                .Include(d => d.AkCarta)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPV1> Insert(AkPV1 entity)
        {
            await context.AkPV1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPV1 entity)
        {

            AkPV1 data = context.AkPV1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
