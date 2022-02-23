using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkBankRepository : IRepository<AkBank, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkBankRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBank.FirstOrDefaultAsync(b => b.Id == id);
            if(model != null)
            {
                context.Remove(model);
            }

        }

        public async Task<IEnumerable<AkBank>> GetAll()
        {
            
            return await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .ToListAsync();
            
            
        }

        public Task<IEnumerable<AkBank>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkBank> GetById(int id)
        {
            return await context.AkBank
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.AkCarta)
                .Where(d=> d.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkBank> GetByIdForDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkBank> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBank> Insert(AkBank entity)
        {
            await context.AkBank.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBank entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
