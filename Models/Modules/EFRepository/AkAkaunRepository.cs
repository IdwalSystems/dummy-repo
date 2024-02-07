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
    public class AkAkaunRepository : IRepository<AkAkaun, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkAkaunRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.AkAkaun.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkAkaun>> GetAll()
        {
            return await context.AkAkaun
                .Include(b => b.JKW)
                .Include(b => b.JBahagian)
                .Include(b => b.AkCarta1)
                .Include(b => b.AkCarta2)
                .ToListAsync();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AkAkaun>> GetAllIncludeDeletedItems()
        {
            throw new NotImplementedException();
        }

        public async Task<AkAkaun> GetById(int id)
        {
            return await context.AkAkaun.FindAsync(id);
        }

        public Task<AkAkaun> GetByIdIncludeDeletedItems(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AkAkaun> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public async Task<AkAkaun> Insert(AkAkaun entity)
        {
            await context.AkAkaun.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkAkaun entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
