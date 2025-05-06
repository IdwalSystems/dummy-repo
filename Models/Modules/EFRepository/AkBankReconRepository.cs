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
    public class AkBankReconRepository : IRepository<AkBankRecon, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkBankReconRepository(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task Delete(int id)
        {
            var akRecon = await context.AkBankRecon.FirstOrDefaultAsync(b => b.Id == id);
            if (akRecon != null)
            {
                context.Remove(akRecon);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkBankRecon>> GetAll(string filter)
        {
            var result = new List<AkBankRecon>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkBankRecon
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .ToListAsync();
            }
            else
            {
                result = await context.AkBankRecon
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;

        }
        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AkBankRecon>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkBankRecon>> GetAllIncludeDeletedItems()
        {
            return await context.AkBankRecon
                .IgnoreQueryFilters()
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkJurnal)
                .ToListAsync();
        }

        public Task<IEnumerable<AkBankRecon>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBankRecon> GetById(int id)
        {
            return await context.AkBankRecon
                .IgnoreQueryFilters()
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkJurnal)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<AkBankRecon> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkBankRecon
                .IgnoreQueryFilters()
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkPV)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkTerima2)
                            .ThenInclude(b => b.AkTerima)
                .Include(b => b.AkBankReconPenyataBank)
                    .ThenInclude(b => b.AkPadananPenyata)
                        .ThenInclude(b => b.AkJurnal)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task<AkBankRecon> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaStringList(bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBankRecon> Insert(AkBankRecon entity)
        {
            await context.AkBankRecon.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBankRecon entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
