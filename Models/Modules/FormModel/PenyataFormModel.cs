using System;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules.FormModel
{
    public class PenyataFormModel
    {
        [Display(Name = "Kumpulan Wang")]
        public int JKWId { get; set; }
        [Display(Name = "Bahagian")]
        public int JBahagianId { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan")]
        public string Tahun1 { get; set; }
        public string Tahun2 { get; set; }
        public string Tahun3 { get; set; }
        public string BulanDari { get; set; }
        public string BulanHingga { get; set; }
        [Display(Name = "Tarikh Dari")]
        public DateTime TarDari1 { get; set; }
        [Display(Name = "Tarikh Hingga")]
        public DateTime TarHingga1 { get; set; }
        [Display(Name = "Bank")]
        public int AkBankId { get; set; }
        [Display(Name = "Paras")]
        public int ParasId { get; set; }

        
    }
}
