using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<AkCimbEFT>> GetAllIncludeDeletedItems()
        {
            return await context.AkCimbEFT
                .IgnoreQueryFilters()
                .Include(b => b.AkCimbEFT1)
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

        public async Task<AkCimbEFT> GetById(int id)
        {
            return await context.AkCimbEFT
                .Include(b => b.AkCimbEFT1)
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
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkCimbEFT> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkCimbEFT
                .IgnoreQueryFilters()
                .Include(b => b.AkCimbEFT1)
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
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkCimbEFT> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkCimbEFT> Insert(AkCimbEFT entity)
        {
            await context.AkCimbEFT.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkCimbEFT entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
