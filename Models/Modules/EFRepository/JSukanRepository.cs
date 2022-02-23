using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class JSukanRepository : IRepository<JSukan, int, string>
    {
        public readonly ApplicationDbContext context;

        public JSukanRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JSukan.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JSukan>> GetAll()
        {
            return await context.JSukan.ToListAsync();
        }

        public Task<IEnumerable<JSukan>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<JSukan> GetById(int id)
        {
            return await context.JSukan.FindAsync(id);

        }

        public Task<JSukan> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<JSukan> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JSukan> Insert(JSukan entity)
        {
            await context.JSukan.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JSukan entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
