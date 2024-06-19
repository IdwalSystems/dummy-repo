using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPenghutangRepository : IRepository<AkPenghutang, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkPenghutangRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var akPenghutang = await context.AkPenghutang.FirstOrDefaultAsync(b => b.Id == id);
            if (akPenghutang != null)
            {
                context.Remove(akPenghutang);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkPenghutang>> GetAll(string filter)
        {
            var result = new List<AkPenghutang>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkPenghutang
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
            }
            else
            {
                result = await context.AkPenghutang
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .Where(b => b.NamaSykt == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkPenghutang>> GetAllIncludeDeletedItems()
        {
            return await context.AkPenghutang
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public async Task<AkPenghutang> GetById(int id)
        {
            return await context.AkPenghutang
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkPenghutang> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPenghutang
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<AkPenghutang> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkPenghutang> Insert(AkPenghutang entity)
        {
            await context.AkPenghutang.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPenghutang entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
