using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
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

        public async Task<decimal> GetBalanceFromAbBukuVot(string tahun, int? akCartaId, int jKWId, int? jBahagianId)
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

        public async Task<decimal> GetBalanceFromKaunterPanjar(string bakiAwal, int akTunaiRuncitId)
        {
            // baki awal
            List<AkTunaiLejar> tunaiLejar = await context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == akTunaiRuncitId && b.Rekup == bakiAwal)
                .OrderBy(b => b.Tarikh)
                .ToListAsync();

            // rekupan
            List<AkTunaiLejar> tunaiLejarRekup = await context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == akTunaiRuncitId && b.Rekup != bakiAwal && b.Rekup != null)
                .OrderBy(b => b.Rekup).ThenBy(b => b.Tarikh)
                .ToListAsync();

            tunaiLejar.AddRange(tunaiLejarRekup);
            // belum rekup
            List<AkTunaiLejar> tunaiLejarBelumRekup = await context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == akTunaiRuncitId && b.Rekup == null)
                .OrderBy(b => b.Tarikh)
                .ToListAsync();

            tunaiLejar.AddRange(tunaiLejarBelumRekup);

            decimal baki = 0;

            if (tunaiLejar != null)
            {
                foreach (var balance in tunaiLejar)
                {
                    baki = baki + balance.Debit - balance.Kredit;
                }

            }

            return baki;
        }


    }
}
