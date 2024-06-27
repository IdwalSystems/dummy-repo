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
    public class AkCimbEFTRepository : IRepository<AkCimbEFT, int, string>
    {
        public readonly ApplicationDbContext context;
        public AkCimbEFTRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkCimbEFT.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkCimbEFT>> GetAll(string filter)
        {
            var result = new List<AkCimbEFT>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkCimbEFT
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                .ToListAsync();
            }
            else
            {
                result = await context.AkCimbEFT
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                .Where(b => b.NamaFail == filter)
                .ToListAsync();
            }

            return result;

        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<AkCimbEFT>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkCimbEFT>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkCimbEFT
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.AkPV)
                                    .ThenInclude(b => b.SuProfil)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.AkPV)
                                    .ThenInclude(b => b.SpPendahuluanPelbagai)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.AkPembekal)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuPekerja)
                                    .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuPekerja)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuAtlet)
                                    .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuAtlet)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuJurulatih)
                                    .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuJurulatih)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkBank)
                                .ThenInclude(b => b.JBank)
                            .Include(b => b.SuPekerja)
                            .Where(b => b.TarJana.Year == DateTime.Now.Year)
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoPBI":
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.NoPBI.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Penjana":
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.SuPekerja.Nama.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.TarJana.Year.ToString() == filter)
                                    .ToListAsync();
                        break;

                    case "TarJana":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(x => x.TarJana >= date1
                                                && x.TarJana <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<IEnumerable<AkCimbEFT>> GetAllIncludeDeletedItems()
        {
            return await context.AkCimbEFT
                .IgnoreQueryFilters()
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                .ToListAsync();
        }

        public async Task<IEnumerable<AkCimbEFT>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            var result = new List<AkCimbEFT>();

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && string.IsNullOrEmpty(filterType)
                || string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(filterDate1) && !string.IsNullOrEmpty(filterType))
            {
                result = await context.AkCimbEFT
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.AkPV)
                                    .ThenInclude(b => b.SuProfil)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.AkPV)
                                    .ThenInclude(b => b.SpPendahuluanPelbagai)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.AkPembekal)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuPekerja)
                                    .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuPekerja)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuAtlet)
                                    .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuAtlet)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuJurulatih)
                                    .ThenInclude(b => b.JCaraBayar)
                            .Include(b => b.AkCimbEFT1)
                                .ThenInclude(b => b.SuJurulatih)
                                    .ThenInclude(b => b.JBank)
                            .Include(b => b.AkBank)
                                .ThenInclude(b => b.JBank)
                            .Include(b => b.SuPekerja)
                            .Where(b => b.TarJana.Year == DateTime.Now.Year)
                            .ToListAsync();

            }

            if ((!string.IsNullOrWhiteSpace(filter) || !string.IsNullOrWhiteSpace(filterDate1)) && !string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "NoPBI":
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.NoPBI.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Penjana":
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.SuPekerja.Nama.ToUpper().Contains(filter.ToUpper()))
                                    .ToListAsync();
                        break;
                    case "Tahun":
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(s => s.TarJana.Year.ToString() == filter)
                                    .ToListAsync();
                        break;

                    case "TarJana":
                        DateTime date1 = DateTime.Parse(filterDate1);
                        DateTime date2 = DateTime.Parse(filterDate2).AddHours(23.99);
                        result = await context.AkCimbEFT
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SuProfil)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPV)
                                            .ThenInclude(b => b.SpPendahuluanPelbagai)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.AkPembekal)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuPekerja)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuAtlet)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JCaraBayar)
                                    .Include(b => b.AkCimbEFT1)
                                        .ThenInclude(b => b.SuJurulatih)
                                            .ThenInclude(b => b.JBank)
                                    .Include(b => b.AkBank)
                                        .ThenInclude(b => b.JBank)
                                    .Include(b => b.SuPekerja)
                                    .Where(x => x.TarJana >= date1
                                                && x.TarJana <= date2)
                                    .ToListAsync();
                        break;
                }

            }

            return result
                    .ToList();
        }

        public async Task<AkCimbEFT> GetById(int id)
        {
            return await context.AkCimbEFT
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkCimbEFT> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkCimbEFT
                .IgnoreQueryFilters()
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkCimbEFT1)
                    .ThenInclude(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkBank)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkCimbEFT> GetByString(string id)
        {
            throw new System.NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AkCimbEFT> Insert(AkCimbEFT entity)
        {
            await context.AkCimbEFT.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkCimbEFT entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
