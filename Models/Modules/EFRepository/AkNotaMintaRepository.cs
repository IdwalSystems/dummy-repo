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
    public class AkNotaMintaRepository : IRepository<AkNotaMinta, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkNotaMintaRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkNotaMinta.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkNotaMinta>> GetAll(string filter)
        {
            var result = new List<AkNotaMinta>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkNotaMinta
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta1)
                .Include(b => b.AkNotaMinta2)
                .ToListAsync();
            }
            else
            {
                result = await context.AkNotaMinta
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta1)
                .Include(b => b.AkNotaMinta2)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkNotaMinta>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkNotaMinta>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkNotaMinta
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.AkNotaMinta1)
                            .Include(b => b.AkNotaMinta2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "NoSiri":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.NoSiri.Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.AkPembekal.NamaSykt.Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkNotaMinta>> GetAllIncludeDeletedItems()
        {
            return await context.AkNotaMinta
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta1)
                .Include(b => b.AkNotaMinta2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkNotaMinta>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkNotaMinta>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkNotaMinta.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.AkNotaMinta1)
                            .Include(b => b.AkNotaMinta2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "NoSiri":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.NoSiri.Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.AkPembekal.NamaSykt.Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkNotaMinta.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta1)
                                    .Include(b => b.AkNotaMinta2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkNotaMinta> GetById(int id)
        {
            return await context.AkNotaMinta
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal).ThenInclude(b=> b.JBank)
                .Include(b => b.AkPembekal).ThenInclude(b => b.JNegeri)
                .Include(b => b.AkNotaMinta1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaMinta2)
                .FirstOrDefaultAsync();
        }

        public async Task<AkNotaMinta> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkNotaMinta
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal).ThenInclude(b => b.JBank)
                .Include(b => b.AkPembekal).ThenInclude(b => b.JNegeri)
                .Include(b => b.AkNotaMinta1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkNotaMinta2)
                .FirstOrDefaultAsync();
        }

        public Task<AkNotaMinta> GetByString(string id)
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

        public async Task<AkNotaMinta> Insert(AkNotaMinta entity)
        {
            await context.AkNotaMinta.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkNotaMinta entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
