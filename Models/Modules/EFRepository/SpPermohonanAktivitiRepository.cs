using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    
    public class SpPermohonanAktivitiRepository : IRepository<SpPermohonanAktiviti, int, string>
    {
        public readonly ApplicationDbContext context;

        public SpPermohonanAktivitiRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SpPermohonanAktiviti.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<SpPermohonanAktiviti>> GetAll()
        {
            return await context.SpPermohonanAktiviti
                .Include(b => b.JKW)
                //.Include(b => b.AkPembekal)
                .Include(b => b.SpPermohonanAktiviti1)
                .Include(b => b.SpPermohonanAktiviti2)
                .ToListAsync();
        }

        public async Task<SpPermohonanAktiviti> GetById(int id)
        {
            return await context.SpPermohonanAktiviti
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(d => d.SpPermohonanAktiviti1).ThenInclude(d => d.AkCarta)
                .Include(d => d.SpPermohonanAktiviti2)
                //.Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                //.Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public Task<SpPermohonanAktiviti> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPermohonanAktiviti> Insert(SpPermohonanAktiviti entity)
        {
            await context.SpPermohonanAktiviti.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPermohonanAktiviti entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
