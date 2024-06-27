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
    public class AkInvoisRepository : IRepository<AkInvois, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkInvoisRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkInvois.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkInvois>> GetAll(string filter)
        {
            var result = new List<AkInvois>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkInvois
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkInvois2)
                .ToListAsync();
            }
            else
            {
                result = await context.AkInvois
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkInvois2)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkInvois>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkInvois>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkInvois
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                            .Include(b => b.KodObjekAP)
                            .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkInvois2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkInvois
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(s => s.NoInbois.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkInvois
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(s => s.AkPenghutang.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkInvois
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkInvois
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkInvois>> GetAllIncludeDeletedItems()
        {
            return await context.AkInvois
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkInvois2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkInvois>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkInvois>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkInvois.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                            .Include(b => b.KodObjekAP)
                            .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkInvois2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkInvois.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(s => s.NoInbois.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkInvois.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(s => s.AkPenghutang.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkInvois.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkInvois.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkInvois2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkInvois> GetById(int id)
        {
            return await context.AkInvois.Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkInvois2)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkInvois> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkInvois.Include(b => b.JKW)
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkPenghutang).ThenInclude(b => b.JNegeri)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkInvois2)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkInvois> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkInvois> Insert(AkInvois entity)
        {
            await context.AkInvois.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkInvois entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
