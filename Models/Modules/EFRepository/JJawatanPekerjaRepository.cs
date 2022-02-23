using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    [Authorize]
    public class JJawatanPekerjaRepository : IRepository<JJawatanPekerja, int, string>
    {
        private readonly ApplicationDbContext context;
        public JJawatanPekerjaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var jwtn = await context.JJawatanPekerja.FirstOrDefaultAsync(b => b.Id == id);
            if (jwtn != null)
            {
                context.Remove(jwtn);
            }
        }

        public async Task<IEnumerable<JJawatanPekerja>> GetAll()
        {
            return await context.JJawatanPekerja.ToListAsync();
        }

        public Task<IEnumerable<JJawatanPekerja>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<JJawatanPekerja> GetById(int id)
        {
            return await context.JJawatanPekerja.FindAsync(id);
        }

        public Task<JJawatanPekerja> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<JJawatanPekerja> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JJawatanPekerja> Insert(JJawatanPekerja entity)
        {
            await context.JJawatanPekerja.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JJawatanPekerja entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
