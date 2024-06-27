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
    public class JKWRepository : IRepository<JKW, int, string>
    {
        private readonly ApplicationDbContext context;
        public JKWRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var kw = await context.JKW.FirstOrDefaultAsync(b => b.Id == id);
            if (kw != null)
            {
                context.Remove(kw);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JKW>> GetAll(string filter)
        {
            return await context.JKW.ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JKW>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JKW>> GetAllIncludeDeletedItems()
        {
            return await context.JKW.IgnoreQueryFilters().ToListAsync();
        }

        public Task<IEnumerable<JKW>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<JKW> GetById(int id)
        {
            return await context.JKW.FindAsync(id);
        }

        public async Task<JKW> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JKW.IgnoreQueryFilters().Where(x=> x.Id == id).FirstOrDefaultAsync();
        }

        public Task<JKW> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<JKW> Insert(JKW entity)
        {
            await context.JKW.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JKW entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
