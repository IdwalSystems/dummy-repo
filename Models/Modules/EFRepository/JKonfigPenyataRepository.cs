using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Helper;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class JKonfigPenyataRepository : IRepository<JKonfigPenyata, int, string>
    {
        public readonly ApplicationDbContext context;

        public JKonfigPenyataRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JKonfigPenyata.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JKonfigPenyata>> GetAll(string filter)
        {
            return await context.JKonfigPenyata.ToListAsync();
        }

        public async Task<IEnumerable<JKonfigPenyata>> GetAllIncludeDeletedItems()
        {
            return await context.JKonfigPenyata
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JKonfigPenyata> GetById(int id)
        {
            var result = await context.JKonfigPenyata.Include(kp => kp.JKonfigPenyataBaris)!.ThenInclude(b => b.JKonfigPenyataBarisFormula)
                .FirstOrDefaultAsync(p => p.Id == id);

            return result ?? new JKonfigPenyata();

        }


        public async Task<JKonfigPenyata> GetByIdIncludeDeletedItems(int id)
        {
            var result = await context.JKonfigPenyata.IgnoreQueryFilters().Include(kp => kp.JKonfigPenyataBaris)!.ThenInclude(b => b.JKonfigPenyataBarisFormula)
                .FirstOrDefaultAsync(p => p.Id == id);

            return result ?? new JKonfigPenyata();
        }

        public Task<JKonfigPenyata> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JKonfigPenyata> Insert(JKonfigPenyata entity)
        {
            await context.JKonfigPenyata.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JKonfigPenyata entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public Task<IEnumerable<JKonfigPenyata>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JKonfigPenyata>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaStringList(bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }
    }
}
