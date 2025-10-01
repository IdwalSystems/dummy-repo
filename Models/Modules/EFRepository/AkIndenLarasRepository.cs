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
    public class AkIndenLarasRepository : IRepository<AkIndenLaras, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkIndenLarasRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkIndenLaras.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkIndenLaras>> GetAll(string filter)
        {
            var result = new List<AkIndenLaras>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkIndenLaras
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkIndenLaras1)
                .Include(b => b.AkIndenLaras2)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkInden1)
                        .ThenInclude(d => d.AkCarta)
                .ToListAsync();
            }
            else
            {
                result = await context.AkIndenLaras
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkIndenLaras1)
                .Include(b => b.AkIndenLaras2)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkInden1)
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

        public async Task<IEnumerable<AkIndenLaras>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkIndenLaras>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkIndenLaras
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkIndenLaras1)
                            .Include(b => b.AkIndenLaras2)
                            .Include(d => d.AkInden)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JBank)
                            .Include(d => d.AkInden)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JNegeri)
                            .Include(d => d.AkInden)
                                .ThenInclude(d => d.AkInden1)
                                    .ThenInclude(d => d.AkCarta)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkIndenLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkIndenLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.AkInden.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkIndenLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkIndenLaras
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
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

        public async Task<IEnumerable<AkIndenLaras>> GetAllIncludeDeletedItems()
        {
            return await context.AkIndenLaras
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkIndenLaras1)
                .Include(b => b.AkIndenLaras2)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkInden1)
                        .ThenInclude(d => d.AkCarta)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkIndenLaras>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkIndenLaras>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkIndenLaras.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkIndenLaras1)
                            .Include(b => b.AkIndenLaras2)
                            .Include(d => d.AkInden)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JBank)
                            .Include(d => d.AkInden)
                                .ThenInclude(d => d.AkPembekal)
                                    .ThenInclude(d => d.JNegeri)
                            .Include(d => d.AkInden)
                                .ThenInclude(d => d.AkInden1)
                                    .ThenInclude(d => d.AkCarta)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkIndenLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkIndenLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.AkInden.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkIndenLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
                                            .ThenInclude(d => d.AkCarta)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkIndenLaras.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkInden).ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkIndenLaras1)
                                    .Include(b => b.AkIndenLaras2)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JBank)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkPembekal)
                                            .ThenInclude(d => d.JNegeri)
                                    .Include(d => d.AkInden)
                                        .ThenInclude(d => d.AkInden1)
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

        public async Task<AkIndenLaras> GetById(int id)
        {
            return await context.AkIndenLaras
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(d => d.AkIndenLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkIndenLaras2)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkInden1)
                        .ThenInclude(d => d.AkCarta)
                .FirstOrDefaultAsync();
        }

        public async Task<AkIndenLaras> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkIndenLaras
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(d => d.AkIndenLaras1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkIndenLaras2)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JBank)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkPembekal)
                        .ThenInclude(d => d.JNegeri)
                .Include(d => d.AkInden)
                    .ThenInclude(d => d.AkInden1)
                        .ThenInclude(d => d.AkCarta)
                .FirstOrDefaultAsync();
        }

        public Task<AkIndenLaras> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaStringList(bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkIndenLaras> Insert(AkIndenLaras entity)
        {
            await context.AkIndenLaras.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkIndenLaras entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
