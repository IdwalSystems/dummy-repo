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
    public class JTahapAktivitiRepository : IRepository<JTahapAktiviti, int, string>
    {
        public readonly ApplicationDbContext context;

        public JTahapAktivitiRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JTahapAktiviti.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JTahapAktiviti>> GetAll()
        {
            return await context.JTahapAktiviti.ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JTahapAktiviti>> GetAllIncludeDeletedItems()
        {
            return await context.JTahapAktiviti
                 .IgnoreQueryFilters()
                 .ToListAsync();
        }

        public async Task<JTahapAktiviti> GetById(int id)
        {
            return await context.JTahapAktiviti.FindAsync(id);

        }

        public async Task<JTahapAktiviti> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JTahapAktiviti
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<JTahapAktiviti> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<JTahapAktiviti> Insert(JTahapAktiviti entity)
        {
            await context.JTahapAktiviti.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JTahapAktiviti entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
