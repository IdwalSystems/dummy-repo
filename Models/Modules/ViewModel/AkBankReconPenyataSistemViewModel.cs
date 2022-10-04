using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkBankReconPenyataSistemViewModel 
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public DateTime Tarikh { get; set; }
        public string NoRujukan { get;set; }
        public string Perihal { get; set; }
        public string NoSlip { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public bool IsGanda { get; set; }
    }
}
