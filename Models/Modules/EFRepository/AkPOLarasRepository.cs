using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkPOLarasRepository : IRepository<AkPOLaras, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPOLarasRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPOLaras.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkPOLaras>> GetAll()
        {
            return await context.AkPOLaras
                .Include(b => b.JKW)
                .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPOLaras1)
                .Include(b => b.AkPOLaras2)
                .ToListAsync();
        }

        public Task<IEnumerable<AkPOLaras>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkPOLaras> GetById(int id)
        {
            return await context.AkPOLaras
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(d => d.AkPOLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPOLaras2)
                .Include(d => d.AkPO).ThenInclude(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .Include(d => d.AkPO).ThenInclude(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .FirstOrDefaultAsync();
        }

        public Task<AkPOLaras> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkPOLaras> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPOLaras> Insert(AkPOLaras entity)
        {
            await context.AkPOLaras.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPOLaras entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
