using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkInvois1Repository : ListViewIRepository<AkInvois1, int>
    {
        public readonly ApplicationDbContext context;

        public AkInvois1Repository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkInvois1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkInvois1>> GetAll(int akInvoisId)
        {
            return await context.AkInvois1.Include(b => b.AkCarta)
                .Where(x => x.AkInvoisId == akInvoisId)
                .ToArrayAsync();
        }

        public async Task<AkInvois1> GetBy2Id(int akInvoisId, int akCartaId)
        {
            return await context.AkInvois1.FirstOrDefaultAsync(x => x.AkInvoisId == akInvoisId && x.AkCartaId == akCartaId);
        }

        public async Task<AkInvois1> GetById(int id)
        {
            return await context.AkInvois1.Include(d => d.AkCarta).Where(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkInvois1> Insert(AkInvois1 entity)
        {
            await context.AkInvois1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInvois1 entity)
        {
            AkInvois1 data = context.AkInvois1.FirstOrDefault(x => x.Id == entity.Id);
            data.Amaun = entity.Amaun;
            await context.SaveChangesAsync();
        }
    }
}
