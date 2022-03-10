using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class CustomRepository : CustomIRepository<string, int>
    {

        public readonly ApplicationDbContext context;
        public CustomRepository(ApplicationDbContext context) => this.context = context;

        public async Task<decimal> GetBalanceFromAbBukuVot(string tahun, int akCartaId, int jKWId, int jBahagianId)
        {
            var sql = (from tbl in await context.AbBukuVot
                       .Include(x => x.Vot)
                       .Include(x => x.JKW)
                       .Include(x => x.JBahagian)
                       .Where(x => x.Tahun == tahun && x.VotId == akCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                       .ToListAsync()
                       select new
                       {
                           Id = tbl.VotId,
                           Tahun = tbl.Tahun,
                           KW = tbl.JKW.Kod,
                           Bahagian = tbl.JBahagian.Kod,
                           KodAkaun = tbl.Vot.Kod,
                           Perihal = tbl.Vot.Perihal,
                           Debit = tbl.Debit,
                           Kredit = tbl.Kredit,
                           Tanggungan = tbl.Tanggungan,
                           Liabiliti = tbl.Liabiliti,
                           Baki = tbl.Baki
                       }).GroupBy(x => new { x.Tahun, x.KodAkaun, x.KW, x.Bahagian }).FirstOrDefault();

            return sql.Select(t => t.Baki + t.Kredit - t.Debit - t.Tanggungan - t.Liabiliti).Sum();
        }
    }
}
