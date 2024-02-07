using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPembekalRepository : IRepository<AkPembekal, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkPembekalRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var akPembekal = await context.AkPembekal.FirstOrDefaultAsync(b => b.Id == id);
            if (akPembekal != null)
            {
                context.Remove(akPembekal);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPembekal>> GetAll()
        {
            return await context.AkPembekal
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPembekal>> GetAllIncludeDeletedItems()
        {
            return await context.AkPembekal
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .ToListAsync();
        }

        public async Task<AkPembekal> GetById(int id)
        {
            return await context.AkPembekal
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkPembekal> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPembekal
                .IgnoreQueryFilters()
                .Include(b => b.JNegeri).Include(b => b.JBank)
                .Where(x=> x.Id == id).FirstOrDefaultAsync();
        }

        public Task<AkPembekal> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPembekal> Insert(AkPembekal entity)
        {
            await context.AkPembekal.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPembekal entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
