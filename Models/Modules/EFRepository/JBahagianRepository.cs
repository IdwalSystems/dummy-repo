using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
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

        public async Task<IEnumerable<JBahagian>> GetAll()
        {
            return await context.JBahagian
                .Include(x=> x.JKW)
                .Include(x => x.JPTJ)
                .ToListAsync();
        }

        public async Task<IEnumerable<JBahagian>> GetAllIncludeDeletedItems()
        {
            return await context.JBahagian
                .IgnoreQueryFilters()
                .Include(x => x.JKW)
                .Include(x => x.JPTJ)
                .ToListAsync();
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
