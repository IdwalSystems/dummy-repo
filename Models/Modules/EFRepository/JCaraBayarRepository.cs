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
    public class JCaraBayarRepository : IRepository<JCaraBayar, int, string>
    {
        public readonly ApplicationDbContext context;
        public JCaraBayarRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var caraBayar = await context.JCaraBayar.FirstOrDefaultAsync(b => b.Id == id);
            if (caraBayar != null)
            {
                context.Remove(caraBayar);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JCaraBayar>> GetAll()
        {
            return await context.JCaraBayar.ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JCaraBayar>> GetAllIncludeDeletedItems()
        {
            return await context.JCaraBayar
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public async Task<JCaraBayar> GetById(int id)
        {
            return await context.JCaraBayar.FindAsync(id);
        }

        public async Task<JCaraBayar> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JCaraBayar
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<JCaraBayar> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<JCaraBayar> Insert(JCaraBayar entity)
        {
            await context.JCaraBayar.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JCaraBayar entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
