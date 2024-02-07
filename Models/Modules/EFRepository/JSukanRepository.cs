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
    public class JSukanRepository : IRepository<JSukan, int, string>
    {
        public readonly ApplicationDbContext context;

        public JSukanRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JSukan.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JSukan>> GetAll()
        {
            return await context.JSukan.ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JSukan>> GetAllIncludeDeletedItems()
        {
            return await context.JSukan
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JSukan> GetById(int id)
        {
            return await context.JSukan.FindAsync(id);

        }

        public async Task<JSukan> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JSukan
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<JSukan> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<JSukan> Insert(JSukan entity)
        {
            await context.JSukan.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JSukan entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
