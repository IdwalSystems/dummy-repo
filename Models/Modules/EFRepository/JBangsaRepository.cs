using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class JBangsaRepository : IRepository<JBangsa, int, string>
    {
        private readonly ApplicationDbContext context;
        public JBangsaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var bangsa = await context.JBangsa.FirstOrDefaultAsync(b => b.Id == id);
            if (bangsa != null)
            {
                context.Remove(bangsa);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JBangsa>> GetAll()
        {
            return await context.JBangsa.ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JBangsa>> GetAllIncludeDeletedItems()
        {
            return await context.JBangsa.IgnoreQueryFilters().ToListAsync();
        }

        public async Task<JBangsa> GetById(int id)
        {
            return await context.JBangsa.FindAsync(id);
        }

        public async Task<JBangsa> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JBangsa.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<JBangsa> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<JBangsa> Insert(JBangsa entity)
        {
            await context.JBangsa.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JBangsa entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
