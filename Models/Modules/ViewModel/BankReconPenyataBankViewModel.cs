using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MSNK.Models.Modules.ViewModel
{
    public class BankReconPenyataBankViewModel 
    {
        public int AkBankReconId { get; set; }
        public string Tahun { get; set; }
        public string Bulan { get;set; }
        [DisplayName("No Akaun")]
        public string NoAkaun { get; set; }
        [DisplayName("Tarikh Kunci")]
        public DateTime? TarKunci { get; set; }
        public AkBankRecon AkBankRecon { get; set; }
        public List<AkBankReconPenyataBank> akBankReconPenyataBank { get; set; }
    }
}
