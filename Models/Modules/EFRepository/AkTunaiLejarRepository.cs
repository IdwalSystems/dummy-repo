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
    public class AkTunaiLejarRepository : IRepository<AkTunaiLejar, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkTunaiLejarRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var model = await context.AkTunaiLejar.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkTunaiLejar>> GetAll(string filter)
        {
            var result = new List<AkTunaiLejar>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkTunaiLejar
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit).ThenInclude(b => b.JKW)
                .Include(b => b.AkCarta)
                .ToListAsync();
            }
            else
            {
                result = await context.AkTunaiLejar
                .Include(b => b.JBahagian)
                .Include(b => b.AkTunaiRuncit).ThenInclude(b => b.JKW)
                .Include(b => b.AkCarta)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AkTunaiLejar>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AkTunaiLejar>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AkTunaiLejar>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiLejar> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiLejar> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkTunaiLejar> GetByString(string id)
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

        public async Task<AkTunaiLejar> Insert(AkTunaiLejar entity)
        {
            await context.AkTunaiLejar.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkTunaiLejar entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
