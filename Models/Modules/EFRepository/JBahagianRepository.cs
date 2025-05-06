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
    public class JBahagianRepository : IRepository<JBahagian, int, string>
    {
        private readonly ApplicationDbContext context;
        public JBahagianRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var obj = await context.JBahagian.FirstOrDefaultAsync(b => b.Id == id);
            if (obj != null)
            {
                context.Remove(obj);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JBahagian>> GetAll(string filter)
        {
            return await context.JBahagian
                .Include(x=> x.JKW)
                .Include(x => x.JPTJ)
                .ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JBahagian>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JBahagian>> GetAllIncludeDeletedItems()
        {
            return await context.JBahagian
                .IgnoreQueryFilters()
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .ToListAsync();
        }

        public Task<IEnumerable<JBahagian>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<JBahagian> GetById(int id)
        {
            return await context.JBahagian
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<JBahagian> GetByIdIncludeDeletedItems(int id)
        {
            return await context.JBahagian
                .IgnoreQueryFilters()
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<JBahagian> GetByString(string id)
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

        public async Task<JBahagian> Insert(JBahagian entity)
        {
            await context.JBahagian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JBahagian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
