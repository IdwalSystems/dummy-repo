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
    public class AkPVRepository : IRepository<AkPV, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkPVRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkPV.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPV>> GetAll(string filter)
        {
            var result = new List<AkPV>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.SuPekerja)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.SuProfil)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                .Include(b => b.AkPV2)
                .ThenInclude(b => b.AkBelian)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .ToListAsync();
            }
            else
            {
                result = await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b => b.AkPembekal)
                .Include(b => b.SuPekerja)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.SuProfil)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                .Include(b => b.AkPV2)
                .ThenInclude(b => b.AkBelian)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .Where(b => b.Tahun == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkPV>> GetAllIncludeDeletedItems()
        {
            return await context.AkPV
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.JBank)
                .Include(b => b.AkPembekal)
                .Include(b => b.SuPekerja)
                .Include(b => b.SpPendahuluanPelbagai)
                .Include(b => b.SuProfil)
                .Include(b => b.AkBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b.AkPO)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b.AkInden)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .ToListAsync();
        }

        public async Task<AkPV> GetById(int id)
        {
            return await context.AkPV
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b=> b.AkTunaiRuncit).ThenInclude(b=> b.AkCarta)
                .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b.AkCarta)
                .Include(b => b.SuProfil).ThenInclude(b=> b.AkCarta)
                .Include(b => b.AkPembekal).ThenInclude(x => x.JBank)
                .Include(b => b.AkPembekal).ThenInclude(x => x.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(x => x.JBank)
                .Include(b => b.SuPekerja).ThenInclude(x => x.JNegeri)
                .Include(b => b.AkBank).ThenInclude(b => b.JBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                    .ThenInclude(b=>b.AkCarta)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b=>b.AkPO)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b.AkInden)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.AkPadananPenyata)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkPV> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkPV
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit).ThenInclude(b => b.AkCarta)
                .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b.AkCarta)
                .Include(b => b.SuProfil).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPembekal).ThenInclude(x => x.JBank)
                .Include(b => b.AkPembekal).ThenInclude(x => x.JNegeri)
                .Include(b => b.SuPekerja).ThenInclude(x => x.JBank)
                .Include(b => b.SuPekerja).ThenInclude(x => x.JNegeri)
                .Include(b => b.AkBank).ThenInclude(b => b.JBank)
                .Include(b => b.JCaraBayar)
                .Include(b => b.AkPV1)
                    .ThenInclude(b => b.AkCarta)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b.AkPO)
                .Include(b => b.AkPV2)
                    .ThenInclude(b => b.AkBelian)
                        .ThenInclude(b => b.AkInden)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuAtlet)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.SuPekerja)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JBank)
                .Include(b => b.AkPVGanda)
                    .ThenInclude(b => b.JCaraBayar)
                .Where(b => b.Id == id)
                .Include(b => b.AkPadananPenyata)
                .FirstOrDefaultAsync();
        }

        public Task<AkPV> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkPV> Insert(AkPV entity)
        {
            await context.AkPV.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkPV entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
