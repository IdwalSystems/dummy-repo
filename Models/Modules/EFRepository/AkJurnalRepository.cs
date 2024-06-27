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
    public class AkJurnalRepository : IRepository<AkJurnal, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkJurnalRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkJurnal.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkJurnal>> GetAll(string filter)
        {
            var result = new List<AkJurnal>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkJurnal
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .ToListAsync();
            }
            else
            {
                result = await context.AkJurnal
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkJurnal>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkJurnal>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkJurnal
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                            .Where(b => b.Tarikh.Year == DateTime.Now.Year)
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                    case "NoJurnal":
                        result = await context.AkJurnal
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(s => s.NoJurnal.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;

                    case "KW":
                        result = await context.AkJurnal
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(s => s.JKW.Kod == filter)
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkJurnal
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(s => s.Tarikh.Year.ToString() == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkJurnal
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkJurnal>> GetAllIncludeDeletedItems()
        {
            return await context.AkJurnal
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkJurnal>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkJurnal>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkJurnal.IgnoreQueryFilters()
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                            .Where(b => b.Tarikh.Year == DateTime.Now.Year)
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                    case "NoJurnal":
                        result = await context.AkJurnal.IgnoreQueryFilters()
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(s => s.NoJurnal.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;

                    case "Catatan":
                        result = await context.AkJurnal.IgnoreQueryFilters()
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(s => s.Catatan1.ToUpper().Contains(filter.ToUpper())
                                            )
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkJurnal.IgnoreQueryFilters()
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(s => s.Tarikh.Year.ToString() == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkJurnal.IgnoreQueryFilters()
                                        .Include(b => b.JBahagian)
                                        .Include(b => b.AkTunaiRuncit)
                                        .Include(b => b.JKW)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaDebit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.JBahagianKredit)
                                        .Include(b => b.AkJurnal1)
                                            .ThenInclude(b => b.AkCartaKredit)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkJurnal> GetById(int id)
        {
            return await context.AkJurnal
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.AkPadananPenyata)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkTunaiRuncit)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AkJurnal> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkJurnal
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.JKW)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkTunaiRuncit)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaDebit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.JBahagianKredit)
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCartaKredit)
                .Where(x=>x.Id == id).FirstOrDefaultAsync();
        }

        public Task<AkJurnal> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkJurnal> Insert(AkJurnal entity)
        {
            await context.AkJurnal.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkJurnal entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
