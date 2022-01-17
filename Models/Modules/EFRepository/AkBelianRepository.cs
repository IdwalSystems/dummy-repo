using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkBelianRepository : IRepository<AkBelian, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkBelianRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBelian.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkBelian>> GetAll()
        {
            return await context.AkBelian
                .Include(b => b.JKW)
                .Include(b => b.AkPO)
                .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1)
                .Include(b => b.AkBelian2)
                .ToListAsync();
        }

        public async Task<AkBelian> GetById(int id)
        {
            return await context.AkBelian.Include(b => b.JKW)
                .Include(b => b.AkPO)
                .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1)
                .Include(b => b.AkBelian2)
                .Where(b=> b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkBelian> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBelian> Insert(AkBelian entity)
        {
            await context.AkBelian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBelian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
