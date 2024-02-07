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
    
    public class AkPORepository : IRepository<AkPO, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPORepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkPO.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPO>> GetAll()
        {
            return await context.AkPO
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPO>> GetAllIncludeDeletedItems()
        {
            return await context.AkPO
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
        }

        public async Task<AkPO> GetById(int id)
        {
            return await context.AkPO
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b=> b.AkNotaMinta)
                .Include(d => d.AkPO1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPO2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPO> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPO
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkPO1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPO2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public Task<AkPO> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPO> Insert(AkPO entity)
        {
            await context.AkPO.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
