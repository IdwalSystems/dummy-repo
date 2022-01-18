using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTunaiPanjarRepository : IRepository<AkTunaiPanjar, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkTunaiPanjarRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTunaiPanjar.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTunaiPanjar>> GetAll()
        {
            return await context.AkTunaiPanjar
                .Include(b => b.JKW)
                .Include(b => b.AkBank).ThenInclude(b=>b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .ToListAsync();
        }

        public Task<AkTunaiPanjar> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiPanjar> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiPanjar> Insert(AkTunaiPanjar entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task Update(AkTunaiPanjar entity)
        {
            throw new NotImplementedException();
        }
    }
}
