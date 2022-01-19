using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTunaiPanjarRepository : IRepository<AkTunaiRuncit, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkTunaiPanjarRepository(ApplicationDbContext context) => this.context = context;
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

        public Task<AkTunaiRuncit> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiRuncit> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiRuncit> Insert(AkTunaiRuncit entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task Update(AkTunaiRuncit entity)
        {
            throw new NotImplementedException();
        }
    }
}
