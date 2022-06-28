using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkCimbEFT1Repository : ListViewIRepository<AkCimbEFT1, int>
    {
        public readonly ApplicationDbContext context;

        public AkCimbEFT1Repository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkCimbEFT1.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkCimbEFT1>> GetAll(int akCimbEFTid)
        {
            return await context.AkCimbEFT1
                .Where(x => x.AkCimbEFTId == akCimbEFTid)
                .ToArrayAsync();
        }

        public async Task<AkCimbEFT1> GetBy2Id(int akCimbEFTId, int indek)
        {
            return await context.AkCimbEFT1.FirstOrDefaultAsync(x => x.AkCimbEFTId == akCimbEFTId && x.Indek == indek);
        }

        public async Task<AkCimbEFT1> GetById(int id)
        {
            return await context.AkCimbEFT1.FindAsync(id);
        }

        public async Task<AkCimbEFT1> Insert(AkCimbEFT1 entity)
        {
            await context.AkCimbEFT1.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkCimbEFT1 entity)
        {
            AkCimbEFT1 data = context.AkCimbEFT1.FirstOrDefault(x => x.Id == entity.Id);
            //data.AkPVId = entity.AkPVId;
            //data.FlPenerimaEFT = entity.FlPenerimaEFT;
            //data.AkPembekalId = entity.AkPembekalId;
            //data.SuPekerjaId = entity.SuPekerjaId;
            //data.SuAtletId = entity.SuAtletId;
            //data.SuJurulatihId = entity.SuJurulatihId;
            //data.Amaun = entity.Amaun;
            //data.NoCek = entity.NoCek;
            //data.Catatan = entity.NoCek;
            //data.JBankId = entity.JBankId;
            //data.FlStatus = entity.FlStatus;
            await context.SaveChangesAsync();
        }
    }
}
