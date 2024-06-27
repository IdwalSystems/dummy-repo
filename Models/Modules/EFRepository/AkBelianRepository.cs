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
    public class AkBelianRepository : IRepository<AkBelian, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkBelianRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkBelian.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkBelian>> GetAll(string filter)
        {
            var result = new List<AkBelian>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkBelian
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .ToListAsync();
            }
            else
            {
                result = await context.AkBelian
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkBelian>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkBelian>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkBelian
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPO)
                                .ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkInden)
                                .ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.KodObjekAP)
                            .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkBelian2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkBelian
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkBelian
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkBelian
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkBelian
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkBelian>> GetAllIncludeDeletedItems()
        {
            return await context.AkBelian
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkBelian>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkBelian>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkBelian.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AkPO)
                                .ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkInden)
                                .ThenInclude(b => b.AkPembekal)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.KodObjekAP)
                            .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                            .Include(b => b.AkBelian2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkBelian.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkBelian.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkBelian.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkBelian.IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AkPO)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkInden)
                                        .ThenInclude(b => b.AkPembekal)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.KodObjekAP)
                                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AkBelian2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkBelian> GetById(int id)
        {
            return await context.AkBelian.Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1).ThenInclude(b=> b.AkCarta)
                .Include(b => b.AkBelian2)
                .Where(b=> b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkBelian> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkBelian.Include(b => b.JKW)
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.AkPO)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkInden)
                    .ThenInclude(b => b.AkPembekal)
                .Include(b => b.AkPembekal)
                .Include(b => b.KodObjekAP)
                .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkBelian> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkBelian> Insert(AkBelian entity)
        {
            await context.AkBelian.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkBelian entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
