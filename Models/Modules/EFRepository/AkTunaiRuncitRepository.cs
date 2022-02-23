using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTunaiRuncitRepository : IRepository<AkTunaiRuncit, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkTunaiRuncitRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTunaiRuncit.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTunaiRuncit>> GetAll()
        {
            return await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .ToListAsync();
        }

        public Task<IEnumerable<AkTunaiRuncit>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkTunaiRuncit> GetById(int id)
        {
            return await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkTunaiRuncit> GetByIdForDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiRuncit> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkTunaiRuncit> Insert(AkTunaiRuncit entity)
        {
            await context.AkTunaiRuncit.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTunaiRuncit entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
