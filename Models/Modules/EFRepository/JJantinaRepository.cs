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
    public class JJantinaRepository : IRepository<JJantina, int, string>
    {
        private readonly ApplicationDbContext context;
        public JJantinaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var jantina = await context.JJantina.FirstOrDefaultAsync(b => b.Id == id);
            if (jantina != null)
            {
                context.Remove(jantina);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JJantina>> GetAll(string filter)
        {
            return await context.JJantina.ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JJantina>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JJantina>> GetAllIncludeDeletedItems()
        {
            return await context.JJantina
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public Task<IEnumerable<JJantina>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<JJantina> GetById(int id)
        {
            return await context.JJantina.FindAsync(id);
        }

        public async Task<JJantina> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JJantina   
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<JJantina> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaStringList(bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<JJantina> Insert(JJantina entity)
        {
            await context.JJantina.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JJantina entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
