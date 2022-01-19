using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTunaiCVRepository : IRepository<AkTunaiCV, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkTunaiCVRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkTunaiCV.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkTunaiCV>> GetAll()
        {
            return await context.AkTunaiCV
                .Include(b => b.SuPekerja)
                .Include(b => b.AkPembekal)
                .Include(b=> b.AkTunaiRuncit).ThenInclude(b=> b.AkTunaiPemegang).ThenInclude(b=> b.SuPekerja)
                
                .Include(b => b.AkTunaiCV1).ThenInclude(b=> b.AkCarta)
                .ToListAsync();
        }

        public Task<AkTunaiCV> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiCV> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiCV> Insert(AkTunaiCV entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task Update(AkTunaiCV entity)
        {
            throw new NotImplementedException();
        }
    }
}
