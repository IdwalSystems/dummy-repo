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
    public class AkBankRepository : IRepository<AkBank, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkBankRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBank.FirstOrDefaultAsync(b => b.Id == id);
            if(model != null)
            {
                context.Remove(model);
            }

        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkBank>> GetAll(string filter)
        {
            var result = new List<AkBank>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .ToListAsync();
            }
            else
            {
                result = await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .Where(b => b.Kod == filter)
                .ToListAsync();
            }

            return result;

            
            
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AkBank>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkBank> GetById(int id)
        {
            return await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .Where(d=> d.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkBank> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkBank> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBank> Insert(AkBank entity)
        {
            await context.AkBank.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBank entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
