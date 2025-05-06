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
    public class AkTerimaRepository : IRepository<AkTerima, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkTerimaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkTerima.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkTerima>> GetAll(string filter)
        {
            var result = new List<AkTerima>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .ToListAsync();
            }
            else
            {
                result = await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkTerima>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkTerima>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkTerima
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.SpPendahuluanPelbagai)
                            .Include(b => b.AkBank)
                            .Include(b => b.JNegeri)
                            .Include(b => b.AkTerima1)
                                .ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkTerima2)
                                .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkTerima3)
                                .ThenInclude(b => b.AkInvois)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkTerima
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkTerima
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(s => s.Nama.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkTerima
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkTerima
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkTerima>> GetAllIncludeDeletedItems()
        {
            return await context.AkTerima
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkTerima>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkTerima>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkTerima.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.SpPendahuluanPelbagai)
                            .Include(b => b.AkBank)
                            .Include(b => b.JNegeri)
                            .Include(b => b.AkTerima1)
                                .ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkTerima2)
                                .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkTerima3)
                                .ThenInclude(b => b.AkInvois)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkTerima.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkTerima.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(s => s.Nama.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkTerima.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkTerima.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkBank)
                                    .Include(b => b.JNegeri)
                                    .Include(b => b.AkTerima1)
                                        .ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkTerima2)
                                        .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkTerima3)
                                        .ThenInclude(b => b.AkInvois)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkTerima> GetById(int id)
        {
            return await context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .Where(b=> b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkTerima> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkTerima
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkBank)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkTerima3)
                    .ThenInclude(b => b.AkInvois)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkTerima> GetByString(string id)
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

        public async Task<AkTerima> Insert(AkTerima entity)
        {
            await context.AkTerima.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTerima entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
