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
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [Display(Name = "Tarikh Dari")]
        public DateTime TarDari { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [Display(Name = "Tarikh Hingga")]
        public DateTime TarHingga { get; set; }
        [Display(Name = "Bank")]
        public int AkBankId { get; set; }
        [Display(Name = "Paras")]
        public int ParasId { get; set; }
    }
}
