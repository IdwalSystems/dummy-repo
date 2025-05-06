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
    public class AkPenyataPemungutRepository : IRepository<AkPenyataPemungut, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkPenyataPemungutRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPenyataPemungut.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkPenyataPemungut>> GetAll(string filter)
        {
            var result = new List<AkPenyataPemungut>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkPenyataPemungut
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b.AkTerima)
                .ToListAsync();
            }
            else
            {
                result = await context.AkPenyataPemungut
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b.JKW)
                .Include(b => b.AkPenyataPemungut2)
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

        public async Task<IEnumerable<AkPenyataPemungut>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkPenyataPemungut>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkPenyataPemungut
                            .Include(b => b.SuPekerja)
                            .Include(b => b.JCaraBayar)
                            .Include(b => b.AkBank)
                                .ThenInclude(b => b.JBank)
                            .Include(b => b.AkBank)
                                .ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkPenyataPemungut1)
                                .ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkPenyataPemungut1)
                                .ThenInclude(b => b.JBahagian)
                                    .ThenInclude(b => b.JKW)
                            .Include(b => b.AkPenyataPemungut2)
                                .ThenInclude(b => b.AkTerima2)
                                    .ThenInclude(b => b.AkTerima)
                                        .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoDokumen":
                        result = await context.AkPenyataPemungut
                                    .Include(b => b.SuPekerja)
                                    .Include(b => b.JCaraBayar)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.JBahagian)
                                            .ThenInclude(b => b.JKW)
                                    .Include(b => b.AkPenyataPemungut2)
                                        .ThenInclude(b => b.AkTerima2)
                                            .ThenInclude(b => b.AkTerima)
                                    .Where(s => s.NoDokumen.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkPenyataPemungut
                                    .Include(b => b.SuPekerja)
                                    .Include(b => b.JCaraBayar)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.JBahagian)
                                            .ThenInclude(b => b.JKW)
                                    .Include(b => b.AkPenyataPemungut2)
                                        .ThenInclude(b => b.AkTerima2)
                                            .ThenInclude(b => b.AkTerima)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkPenyataPemungut
                                    .Include(b => b.SuPekerja)
                                    .Include(b => b.JCaraBayar)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.JBahagian)
                                            .ThenInclude(b => b.JKW)
                                    .Include(b => b.AkPenyataPemungut2)
                                        .ThenInclude(b => b.AkTerima2)
                                            .ThenInclude(b => b.AkTerima)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkPenyataPemungut>> GetAllIncludeDeletedItems()
        {
            return await context.AkPenyataPemungut
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b.AkTerima)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPenyataPemungut>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkPenyataPemungut>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkPenyataPemungut.IgnoreQueryFilters()
                            .Include(b => b.SuPekerja)
                            .Include(b => b.JCaraBayar)
                            .Include(b => b.AkBank)
                                .ThenInclude(b => b.JBank)
                            .Include(b => b.AkBank)
                                .ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkPenyataPemungut1)
                                .ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkPenyataPemungut1)
                                .ThenInclude(b => b.JBahagian)
                                    .ThenInclude(b => b.JKW)
                            .Include(b => b.AkPenyataPemungut2)
                                .ThenInclude(b => b.AkTerima2)
                                    .ThenInclude(b => b.AkTerima)
                                        .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoDokumen":
                        result = await context.AkPenyataPemungut.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Include(b => b.JCaraBayar)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.JBahagian)
                                            .ThenInclude(b => b.JKW)
                                    .Include(b => b.AkPenyataPemungut2)
                                        .ThenInclude(b => b.AkTerima2)
                                            .ThenInclude(b => b.AkTerima)
                                    .Where(s => s.NoDokumen.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkPenyataPemungut.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Include(b => b.JCaraBayar)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.JBahagian)
                                            .ThenInclude(b => b.JKW)
                                    .Include(b => b.AkPenyataPemungut2)
                                        .ThenInclude(b => b.AkTerima2)
                                            .ThenInclude(b => b.AkTerima)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkPenyataPemungut.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Include(b => b.JCaraBayar)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkPenyataPemungut1)
                                        .ThenInclude(b => b.JBahagian)
                                            .ThenInclude(b => b.JKW)
                                    .Include(b => b.AkPenyataPemungut2)
                                        .ThenInclude(b => b.AkTerima2)
                                            .ThenInclude(b => b.AkTerima)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkPenyataPemungut> GetById(int id)
        {
            return await context.AkPenyataPemungut
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b.AkTerima)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPenyataPemungut> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPenyataPemungut
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPenyataPemungut1)
                    .ThenInclude(b => b.JBahagian)
                        .ThenInclude(b => b.JKW)
                .Include(b => b.AkPenyataPemungut2)
                    .ThenInclude(b => b.AkTerima2)
                        .ThenInclude(b => b.AkTerima)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkPenyataPemungut> GetByString(string id)
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

        public async Task<AkPenyataPemungut> Insert(AkPenyataPemungut entity)
        {
            await context.AkPenyataPemungut.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPenyataPemungut entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
