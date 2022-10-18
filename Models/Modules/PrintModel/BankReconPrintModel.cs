using MSNK.Models.Administration;
using System;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel
{
    public class BankReconPrintModel
    {
        public DateTime Tarikh { get; set; }
        public string Bank { get; set; }
        public decimal bakiPenyata { get; set; }
        public decimal bakiBukuTunai { get; set; }
        public List<CashBookDetails> bayaranBelumJelasPenyataBank { get; set; } = new List<CashBookDetails>();
        public decimal JumBayaranBelumJelasPenyataBank { get; set; }
        public List<AkBankReconPenyataBank> bayaranBelumAkuiBukuTunai { get; set; } = new List<AkBankReconPenyataBank>();
        public decimal JumBayaranBelumAkuiBukuTunai { get; set; }
        public List<CashBookDetails> terimaanBelumJelasPenyataBank { get; set; } = new List<CashBookDetails>();
        public decimal JumTerimaanBelumJelasPenyataBank { get; set; }
        public List<AkBankReconPenyataBank> terimaanBelumAkuiBukuTunai { get; set; } = new List<AkBankReconPenyataBank>();
        public decimal JumTerimaanBelumAkuiBukuTunai { get; set; }
        public decimal Beza { get; set; }
        public CompanyDetails company { get; set; }

    }
    public class CashBookDetails
    {
        public DateTime date { get; set; }
        public string refNo { get; set; }
        public string name { get; set; }
        public string cekNo { get; set; }
        public decimal amount { get; set; }
    }
}
