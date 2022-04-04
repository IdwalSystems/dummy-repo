using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class SuAtletRepository : IRepository<SuAtlet, int, string>
    {
        public readonly ApplicationDbContext context;
        public SuAtletRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SuAtlet.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SuAtlet>> GetAll()
        {
            return await context.SuAtlet
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuAtlet>> GetAllIncludeDeletedItems()
        {
            return await context.SuAtlet
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .ToListAsync();
        }

        public async Task<SuAtlet> GetById(int id)
        {
            return await context.SuAtlet
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<SuAtlet> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SuAtlet
                .IgnoreQueryFilters()
                .Include(b => b.JAgama)
                .Include(b => b.JBangsa)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JNegeri)
                //.Include(b => b.SuTanggungan)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SuAtlet> GetByString(string noKP)
        {
            return await context.SuAtlet.Where(x=>x.NoKp == noKP).FirstOrDefaultAsync();
        }

        public async Task<SuAtlet> Insert(SuAtlet entity)
        {
            await context.SuAtlet.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SuAtlet entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
