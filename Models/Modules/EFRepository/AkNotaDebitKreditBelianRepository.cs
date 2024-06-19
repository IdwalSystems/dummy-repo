using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkNotaDebitKreditBelianRepository : IRepository<AkNotaDebitKreditBelian, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkNotaDebitKreditBelianRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkNotaDebitKreditBelian.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian>> GetAll(string filter)
        {
            var result = new List<AkNotaDebitKreditBelian>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkNotaDebitKreditBelian
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .ToListAsync();
            }
            else
            {
                result = await context.AkNotaDebitKreditBelian
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkNotaDebitKreditBelian>> GetAllIncludeDeletedItems()
        {
            return await context.AkNotaDebitKreditBelian
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .ToListAsync();
        }

        public async Task<AkNotaDebitKreditBelian> GetById(int id)
        {
            return await context.AkNotaDebitKreditBelian
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include( b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<AkNotaDebitKreditBelian> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkNotaDebitKreditBelian
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.KodObjekAP)
                .Include(b => b.AkBelian)
                    .ThenInclude(b => b.AkPO)
                        .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkNotaDebitKreditBelian1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaDebitKreditBelian2)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<AkNotaDebitKreditBelian> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkNotaDebitKreditBelian> Insert(AkNotaDebitKreditBelian entity)
        {
            await context.AkNotaDebitKreditBelian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaDebitKreditBelian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
