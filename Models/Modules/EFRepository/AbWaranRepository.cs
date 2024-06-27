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
    public class AbWaranRepository : IRepository<AbWaran, int, string>
    {
        public readonly ApplicationDbContext context;

        public AbWaranRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AbWaran.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AbWaran>> GetAll(string filter)
        {
            var result = new List<AbWaran>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AbWaran
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .ToListAsync();
            }
            else
            {
                result = await context.AbWaran
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<AbWaran>> GetAllFiltered(string filter,string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AbWaran>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AbWaran
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                            .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AbWaran
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AbWaran
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AbWaran
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }
                
            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AbWaran>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AbWaran>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AbWaran.IgnoreQueryFilters()
                            .Include(b => b.JKW)
                            .Include(b => b.JBahagian)
                            .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                            .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                            .Where(b => b.Tahun == DateTime.Now.Year.ToString())
                            .ToListAsync();

            }

            if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoRujukan":
                        result = await context.AbWaran
                                    .IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                                    .Where(s => s.NoRujukan.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AbWaran
                            .IgnoreQueryFilters()
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                                    .Where(s => s.Tahun == filter)
                                    .ToListAsync();
                        break;

                    case "Tarikh":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AbWaran
                                    .Include(b => b.JKW)
                                    .Include(b => b.JBahagian)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                                    .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                                    .Where(x => x.Tarikh >= date1
                                                && x.Tarikh <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        

        public async Task<IEnumerable<AbWaran>> GetAllIncludeDeletedItems()
        {
            return await context.AbWaran
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .ToListAsync();
        }



        public async Task<AbWaran> GetById(int id)
        {
            return await context.AbWaran
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AbWaran> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AbWaran
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AbWaran1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AbWaran1).ThenInclude(b => b.JBahagian)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AbWaran> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AbWaran> Insert(AbWaran entity)
        {
            await context.AbWaran.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AbWaran entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
