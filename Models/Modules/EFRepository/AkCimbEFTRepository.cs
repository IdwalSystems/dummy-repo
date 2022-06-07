using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkCimbEFTRepository : IRepository<AkCimbEFT, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkCimbEFTRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkCimbEFT.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<AkCimbEFT>> GetAll()
        {
            return await context.AkCimbEFT
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkBank)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                .ToListAsync();
        }

        public Task<IEnumerable<AkCimbEFT>> GetAllIncludeDeletedItems()
        {
            throw new System.NotImplementedException();
        }

        public Task<AkCimbEFT> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AkCimbEFT> GetByIdIncludeDeletedItems(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AkCimbEFT> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AkCimbEFT> Insert(AkCimbEFT entity)
        {
            throw new System.NotImplementedException();
        }

        public Task Save()
        {
            throw new System.NotImplementedException();
        }

        public Task Update(AkCimbEFT entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
