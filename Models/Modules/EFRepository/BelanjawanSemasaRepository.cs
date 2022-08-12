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
    
    public class BelanjawanSemasaRepository : BelanjawanSemasaIRepository<string, int>
    {
        

        public readonly ApplicationDbContext context;
        public BelanjawanSemasaRepository(ApplicationDbContext context) => this.context = context;
        

        decimal Asal = 0;
        decimal Tambah = 0;
        decimal Pindah = 0;
        decimal Jumlah = 0;
        decimal Belanja = 0;
        decimal TBS = 0;
        decimal TelahGuna = 0;
        decimal Baki = 0;
        public async Task<List<AbWaran>> GetAbWaranBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AbWaran
                .Include(b => b.AbWaran1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tahun == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public List<AbBelanjawanSemasaViewModel> RunWaranObjekOperation(int JenisWaran, string TK, decimal Amaun, string KodCarta, string Perihal, string Paras)
        {
            var list = new List<AbBelanjawanSemasaViewModel>();
            if (JenisWaran == 0) // WPA
            {
                Asal = Amaun;
            }
            else if (JenisWaran == 1) // WPT
            {
                Tambah = Amaun;
            }
            else if (JenisWaran == 2) // WPP
            {

                Pindah = Amaun;
            }

            if (TK == "-")
            {
                Jumlah =  0 - Amaun;
            }
            else
            {
                Jumlah = Amaun;
            }

            if (TK == "-")
            {
                Baki =  0 - Amaun;
            }
            else
            {
                Baki = Amaun;
            }

            list.Add(
                        new AbBelanjawanSemasaViewModel
                        {
                            Objek = KodCarta,
                            Perihalan = Perihal,
                            Paras = Paras,
                            Asal = Asal,
                            Tambah = Tambah,
                            Pindah = Pindah,
                            Jumlah = Jumlah,
                            Belanja = 0,
                            TBS = 0,
                            TelahGuna = 0,
                            Baki = Baki
                        }
                    );

            return list;
        }

        public async Task<List<AkPO>> GetAkPOBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkPO
                .Include(b => b.AkPO1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tahun == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public async Task<List<AkPOLaras>> GetAkPOLarasBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkPOLaras
                .Include(b => b.AkPOLaras1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tahun == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }


        public async Task<List<AkInden>> GetAkIndenBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkInden
                .Include(b => b.AkInden1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tahun == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public List<AbBelanjawanSemasaViewModel> RunPOPOLarasIndenCVObjekOperation(decimal Amaun, string KodCarta, string Perihal, string Paras)
        {
            var list = new List<AbBelanjawanSemasaViewModel>();

            TBS = Amaun;
            TelahGuna = Amaun;
            Baki = 0 - Amaun;

            list.Add(
                        new AbBelanjawanSemasaViewModel
                        {
                            Objek = KodCarta,
                            Perihalan = Perihal,
                            Paras = Paras,
                            Asal = Asal,
                            Tambah = Tambah,
                            Pindah = Pindah,
                            Jumlah = Jumlah,
                            Belanja = Belanja,
                            TBS = TBS,
                            TelahGuna = TelahGuna,
                            Baki = Baki
                        }
                    );

            return list;
        }

        public async Task<List<AkPV>> GetAkPVBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkPV
                .Include(b => b.AkPV1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tahun == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public List<AbBelanjawanSemasaViewModel> RunBaucerObjekOperation(bool Tanggungan, decimal Amaun, string KodCarta, string Perihal, string Paras)
        {
            var list = new List<AbBelanjawanSemasaViewModel>();

            if (Tanggungan == true)
            {
                TBS = 0 - Amaun;
            }
            Belanja = Amaun;
            TelahGuna = Amaun;
            Baki = 0 - Amaun;

            list.Add(
                        new AbBelanjawanSemasaViewModel
                        {
                            Objek = KodCarta,
                            Perihalan = Perihal,
                            Paras = Paras,
                            Asal = Asal,
                            Tambah = Tambah,
                            Pindah = Pindah,
                            Jumlah = Jumlah,
                            Belanja = Belanja,
                            TBS = TBS,
                            TelahGuna = TelahGuna,
                            Baki = Baki
                        }
                    );

            return list;
        }

        public async Task<List<AkTunaiCV>> GetAkTunaiCVBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkTunaiCV
                .Include(b => b.AkTunaiRuncit)
                .Include(b => b.AkTunaiCV1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tahun == tahun
                && b.AkTunaiRuncit.JKWId == jKWId
                && b.AkTunaiRuncit.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public async Task<List<AkTerima>> GetAkTerimaBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkTerima
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Include(b => b.AkTerima1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JJenis)
                .Where(
                b => b.Tahun == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public List<AbBelanjawanSemasaViewModel> RunResitObjekOperation(decimal Amaun, string KodCarta, string Perihal, string Paras)
        {
            var list = new List<AbBelanjawanSemasaViewModel>();

            Belanja = 0 - Amaun;
            TelahGuna = 0 - Amaun;
            Baki = Amaun;

            list.Add(
                        new AbBelanjawanSemasaViewModel
                        {
                            Objek = KodCarta,
                            Perihalan = Perihal,
                            Paras = Paras,
                            Asal = Asal,
                            Tambah = Tambah,
                            Pindah = Pindah,
                            Jumlah = Jumlah,
                            Belanja = Belanja,
                            TBS = TBS,
                            TelahGuna = TelahGuna,
                            Baki = Baki
                        }
                    );

            return list;
        }

        public async Task<List<AkJurnal>> GetAkJurnalBasedOnYear(string tahun, int jKWId, int jBahagianId, DateTime tarHingga)
        {
            var sql = await context.AkJurnal
                .Include(b => b.AkJurnal1)
                    .ThenInclude(b => b.AkCarta)
                        .ThenInclude(b => b.JParas)
                .Where(
                b => b.Tarikh.Year.ToString() == tahun
                && b.JKWId == jKWId
                && b.JBahagianId == jBahagianId
                && b.Tarikh <= tarHingga).OrderBy(b => b.Tarikh)
                .ToListAsync();

            return sql;
        }

        public List<AbBelanjawanSemasaViewModel> RunJurnalObjekOperation(decimal Debit, decimal Kredit, string KodCarta, string Perihal, string Paras)
        {
            var list = new List<AbBelanjawanSemasaViewModel>();

            Belanja = Debit - Kredit;
            TelahGuna = Debit - Kredit;
            Baki = Kredit - Debit;

            list.Add(
                        new AbBelanjawanSemasaViewModel
                        {
                            Objek = KodCarta,
                            Perihalan = Perihal,
                            Paras = Paras,
                            Asal = Asal,
                            Tambah = Tambah,
                            Pindah = Pindah,
                            Jumlah = Jumlah,
                            Belanja = Belanja,
                            TBS = TBS,
                            TelahGuna = TelahGuna,
                            Baki = Baki
                        }
                    );

            return list;
        }
    }
}
