using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkIndenRepository : IRepository<AkInden, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkIndenRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkInden.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkInden>> GetAll()
        {
            return await context.AkInden
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkInden1)
                .Include(b => b.AkInden2)
                .ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkInden>> GetAllIncludeDeletedItems()
        {
            return await context.AkInden
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkInden1)
                .Include(b => b.AkInden2)
                .ToListAsync();
        }

        public async Task<AkInden> GetById(int id)
        {
            return await context.AkInden
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkInden1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkInden2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public async Task<AkInden> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkInden
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkInden1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkInden2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public Task<AkInden> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkInden> Insert(AkInden entity)
        {
            await context.AkInden.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInden entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
