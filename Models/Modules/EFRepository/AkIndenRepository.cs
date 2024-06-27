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
    public class AkIndenRepository : IRepository<AkInden, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkIndenRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkInden.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkInden>> GetAll(string filter)
        {
            var result = new List<AkInden>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkInden
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkInden1)
                .Include(b => b.AkInden2)
                .ToListAsync();
            }
            else
            {
                result = await context.AkInden
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkInden1)
                .Include(b => b.AkInden2)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;

        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkInden>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkInden>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkInden
                            .Include(b => b.JBahagian)
                            .Include(b => b.JKW)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.AkNotaMinta)
                            .Include(b => b.AkInden1)
                            .Include(b => b.AkInden2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkInden
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(s => s.NoInden.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkInden
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkInden
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkInden
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkInden>> GetAllIncludeDeletedItems()
        {
            return await context.AkInden
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkInden1)
                .Include(b => b.AkInden2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkInden>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkInden>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkInden.IgnoreQueryFilters()
                            .Include(b => b.JBahagian)
                            .Include(b => b.JKW)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.AkNotaMinta)
                            .Include(b => b.AkInden1)
                            .Include(b => b.AkInden2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkInden.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(s => s.NoInden.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkInden.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkInden.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkInden.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkInden1)
                                    .Include(b => b.AkInden2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkInden> GetById(int id)
        {
            return await context.AkInden
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkInden1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkInden2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public async Task<AkInden> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkInden
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkInden1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkInden2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public Task<AkInden> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkInden> Insert(AkInden entity)
        {
            await context.AkInden.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInden entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
