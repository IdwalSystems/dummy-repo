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
    
    public class SpPendahuluanPelbagaiRepository : IRepository<SpPendahuluanPelbagai, int, string>
    {
        public readonly ApplicationDbContext context;

        public SpPendahuluanPelbagaiRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.SpPendahuluanPelbagai.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai>> GetAll(string filter)
        {
            var result = new List<SpPendahuluanPelbagai>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.SpPendahuluanPelbagai
                .Include(b => b.SuPekerja)
                .ToListAsync();
            }
            else
            {
                result = await context.SpPendahuluanPelbagai
                .Include(b => b.SuPekerja)
                .Where(sp => sp.TarMasuk.Year.ToString() == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<SpPendahuluanPelbagai>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.SpPendahuluanPelbagai
                            .Include(b => b.SuPekerja)
                            .Where(b => b.TarMasuk.Year == DateTime.Now.Year)
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.SpPendahuluanPelbagai
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.NoPermohonan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.SpPendahuluanPelbagai
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.SuPekerja.Nama.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.SpPendahuluanPelbagai
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.TarMasuk.Year.ToString() == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.SpPendahuluanPelbagai
                                    .Include(b => b.SuPekerja)
                                    .Where(x => x.TarMasuk >= date1
                                                && x.TarMasuk <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai>> GetAllIncludeDeletedItems()
        {
            return await context.SpPendahuluanPelbagai
                .IgnoreQueryFilters()
                .Include(b => b.SuPekerja)
                .ToListAsync();
        }

        public async Task<IEnumerable<SpPendahuluanPelbagai>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<SpPendahuluanPelbagai>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.SpPendahuluanPelbagai.IgnoreQueryFilters()
                            .Include(b => b.SuPekerja)
                            .Where(b => b.TarMasuk.Year == DateTime.Now.Year)
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.SpPendahuluanPelbagai.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.NoPermohonan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.SpPendahuluanPelbagai.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.SuPekerja.Nama.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.SpPendahuluanPelbagai.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.TarMasuk.Year.ToString() == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.SpPendahuluanPelbagai.IgnoreQueryFilters()
                                    .Include(b => b.SuPekerja)
                                    .Where(x => x.TarMasuk >= date1
                                                && x.TarMasuk <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<SpPendahuluanPelbagai> GetById(int id)
        {
            return await context.SpPendahuluanPelbagai
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JTahapAktiviti)
                .Include(b => b.JSukan)
                .Include(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                .Include(d => d.SpPendahuluanPelbagai2)
                //.Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                //.Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public async Task<SpPendahuluanPelbagai> GetByIdIncludeDeletedItems(int id)
        {
            return await context.SpPendahuluanPelbagai
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JTahapAktiviti)
                .Include(b => b.JSukan)
                .Include(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(b=> b.JCaraBayar)
                .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                .Include(d => d.SpPendahuluanPelbagai2)
                //.Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                //.Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public Task<SpPendahuluanPelbagai> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<SpPendahuluanPelbagai> Insert(SpPendahuluanPelbagai entity)
        {
            await context.SpPendahuluanPelbagai.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(SpPendahuluanPelbagai entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
