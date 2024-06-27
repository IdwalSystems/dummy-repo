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
    
    public class AkPORepository : IRepository<AkPO, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPORepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkPO.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPO>> GetAll(string filter)
        {
            var result = new List<AkPO>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkPO
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
            }
            else
            {
                result = await context.AkPO
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;

        }


        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPO>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkPO>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkPO
                            .Include(b => b.JBahagian)
                            .Include(b => b.JKW)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.AkNotaMinta)
                            .Include(b => b.AkPO1)
                            .Include(b => b.AkPO2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkPO
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(s => s.NoPO.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkPO
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkPO
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkPO
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkPO>> GetAllIncludeDeletedItems()
        {
            return await context.AkPO
                .IgnoreQueryFilters()
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkPembekal)
                .Include(b => b.AkNotaMinta)
                .Include(b => b.AkPO1)
                .Include(b => b.AkPO2)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkPO>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkPO>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkPO.IgnoreQueryFilters()
                            .Include(b => b.JBahagian)
                            .Include(b => b.JKW)
                            .Include(b => b.AkPembekal)
                            .Include(b => b.AkNotaMinta)
                            .Include(b => b.AkPO1)
                            .Include(b => b.AkPO2)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AkPO.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(s => s.NoPO.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Nama":
                        result = await context.AkPO.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkPO.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkPO.IgnoreQueryFilters()
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.JKW)
                                    .Include(b => b.AkPembekal)
                                    .Include(b => b.AkNotaMinta)
                                    .Include(b => b.AkPO1)
                                    .Include(b => b.AkPO2)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkPO> GetById(int id)
        {
            return await context.AkPO
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b=> b.AkNotaMinta)
                .Include(d => d.AkPO1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPO2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPO> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPO
                .IgnoreQueryFilters()
                .Where(d => d.Id == id)
                .Include(b => b.JBahagian)
                .Include(b => b.JKW)
                .Include(b => b.AkNotaMinta)
                .Include(d => d.AkPO1).ThenInclude(d => d.AkCarta)
                .Include(d => d.AkPO2)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JNegeri)
                .Include(d => d.AkPembekal).ThenInclude(d => d.JBank)
                .FirstOrDefaultAsync();
        }

        public Task<AkPO> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPO> Insert(AkPO entity)
        {
            await context.AkPO.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPO entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
