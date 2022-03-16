using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class SuPekerjaRepository : IRepository<SuPekerja, int, string>
    {
        public readonly ApplicationDbContext context;
        public SuPekerjaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuPekerja.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuPekerja>> GetAll()
        {
            return await context.SuPekerja
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuPekerja>> GetAllIncludeDeletedItems()
        {
            return await context.SuPekerja
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<SuPekerja> GetById(int id)
        {
            return await context.SuPekerja
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<SuPekerja> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuPekerja
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                .Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SuPekerja> GetByString(string noKP)
        {
            return await context.SuPekerja.Where(x=>x.NoKp == noKP).FirstOrDefaultAsync();
        }

        public async Task<SuPekerja> Insert(SuPekerja entity)
        {
            await context.SuPekerja.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuPekerja entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
