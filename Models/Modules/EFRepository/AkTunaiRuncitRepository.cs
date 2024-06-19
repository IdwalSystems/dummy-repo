using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkTunaiRuncitRepository : IRepository<AkTunaiRuncit, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkTunaiRuncitRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTunaiRuncit.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkTunaiRuncit>> GetAll(string filter)
        {
            var result = new List<AkTunaiRuncit>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .ToListAsync();
            }
            else
            {
                result = await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .Where(b => b.TarMasuk.ToString("yyyy") == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkTunaiRuncit>> GetAllIncludeDeletedItems()
        {
            return await context.AkTunaiRuncit
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .ToListAsync();
        }

        public async Task<AkTunaiRuncit> GetById(int id)
        {
            return await context.AkTunaiRuncit
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkTunaiLejar)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkTunaiRuncit> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkTunaiRuncit
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta)
                .Include(b => b.AkTunaiPemegang).ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkTunaiLejar)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkTunaiRuncit> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkTunaiRuncit> Insert(AkTunaiRuncit entity)
        {
            await context.AkTunaiRuncit.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTunaiRuncit entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
