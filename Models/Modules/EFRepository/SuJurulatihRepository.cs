using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class SuJurulatihRepository : IRepository<SuJurulatih, int, string>
    {
        public readonly ApplicationDbContext context;
        public SuJurulatihRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuJurulatih.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuJurulatih>> GetAll()
        {
            return await context.SuJurulatih
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuJurulatih>> GetAllIncludeDeletedItems()
        {
            return await context.SuJurulatih
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<SuJurulatih> GetById(int id)
        {
            return await context.SuJurulatih
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<SuJurulatih> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuJurulatih
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SuJurulatih> GetByString(string noKP)
        {
            return await context.SuJurulatih.Where(x=>x.NoKp == noKP).FirstOrDefaultAsync();
        }

        public async Task<SuJurulatih> Insert(SuJurulatih entity)
        {
            await context.SuJurulatih.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuJurulatih entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
