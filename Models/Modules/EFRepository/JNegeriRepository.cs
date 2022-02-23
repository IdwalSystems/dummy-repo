using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class JNegeriRepository : IRepository<JNegeri, int, string>
    {
        public readonly ApplicationDbContext context;

        public JNegeriRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JNegeri.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JNegeri>> GetAll()
        {
            return await context.JNegeri.ToListAsync();
        }

        public Task<IEnumerable<JNegeri>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<JNegeri> GetById(int id)
        {
            return await context.JNegeri.FindAsync(id);

        }

        public Task<JNegeri> GetByIdForDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<JNegeri> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JNegeri> Insert(JNegeri entity)
        {
            await context.JNegeri.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JNegeri entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
