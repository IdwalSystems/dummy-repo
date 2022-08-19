using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkBankReconPenyataBankRepository : ListViewIRepository<AkBankReconPenyataBank, int>
    {
        public readonly ApplicationDbContext context;
        public AkBankReconPenyataBankRepository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var model = await context.AkBankReconPenyataBank.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkBankReconPenyataBank>> GetAll(int akBankReconId)
        {
            return await context.AkBankReconPenyataBank
                .Where(x => x.AkBankReconId == akBankReconId)
                .ToArrayAsync();
        }

        public async Task<AkBankReconPenyataBank> GetBy2Id(int akBankReconId, int indek)
        {
            return await context.AkBankReconPenyataBank.FirstOrDefaultAsync(x => x.AkBankReconId == akBankReconId && x.Indek == indek);
        }

        public async Task<AkBankReconPenyataBank> GetById(int id)
        {
            return await context.AkBankReconPenyataBank.FindAsync(id);
        }

        public async Task<AkBankReconPenyataBank> Insert(AkBankReconPenyataBank entity)
        {
            await context.AkBankReconPenyataBank.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBankReconPenyataBank entity)
        {
            AkBankReconPenyataBank data = await context.AkBankReconPenyataBank.FirstOrDefaultAsync(x => x.Id == entity.Id);
            await context.SaveChangesAsync();
        }
    }
}
