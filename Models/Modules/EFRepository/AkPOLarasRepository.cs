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
    public class AkPOLarasRepository : IRepository<AkPOLaras, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPOLarasRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPOLaras.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPOLaras>> GetAll(string filter)
        {
            var result = new List<AkPOLaras>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkPOLaras
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPOLaras1)
                .Include(b => b.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .ToListAsync();
            }
            else
            {
                result = await context.AkPOLaras
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPOLaras1)
                .Include(b => b.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPOLaras>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkPOLaras>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkPOLaras
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkPOLaras1)
                            .Include(b => b.AkPOLaras2)
                            .Include(d => d.AkPO)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JBank)
                            .Include(d => d.AkPO)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JNegeri)
                            .Include(d => d.AkPO)
                                .ThenInclude(d => d.AkPO1)
                                    .ThenInclude(d => d.AkCarta)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkPOLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkPOLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.AkPO.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkPOLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkPOLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkPOLaras>> GetAllIncludeDeletedItems()
        {
            return await context.AkPOLaras
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPOLaras1)
                .Include(b => b.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPOLaras>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkPOLaras>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkPOLaras.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkPOLaras1)
                            .Include(b => b.AkPOLaras2)
                            .Include(d => d.AkPO)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JBank)
                            .Include(d => d.AkPO)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JNegeri)
                            .Include(d => d.AkPO)
                                .ThenInclude(d => d.AkPO1)
                                    .ThenInclude(d => d.AkCarta)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkPOLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkPOLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.AkPO.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkPOLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkPOLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPOLaras1)
                                    .Include(b => b.AkPOLaras2)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkPO)
                                        .ThenInclude(d => d.AkPO1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkPOLaras> GetById(int id)
        {
            return await context.AkPOLaras
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(d => d.AkPOLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPOLaras> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPOLaras
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(d => d.AkPOLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPOLaras2)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPO)
                    .ThenInclude(d => d.AkPO1)
                        .ThenInclude(d => d.AkCarta)
                .FirstOrDefaultAsync();
        }

        public Task<AkPOLaras> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPOLaras> Insert(AkPOLaras entity)
        {
            await context.AkPOLaras.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPOLaras entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
