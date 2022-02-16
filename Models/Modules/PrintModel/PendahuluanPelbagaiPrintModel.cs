using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class PendahuluanPelbagaiPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public SpPendahuluanPelbagai SpPendahuluanPelbagai { get; set; }
        public List<JTahapAktiviti> Tahap { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
        public int BilAtl {get;set;}
        public int BilJul {get;set;}
        public int BilPeg {get;set;}
        public int BilTek {get;set;}
        public int BilUru {get;set;}
        public int Jumlah { get; set; }
        public decimal JumlahPerihal { get; set; }
    }
}
