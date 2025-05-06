using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Operations
{
    public enum EnKategoriTajuk
    {
        [Display(Name = "Tajuk Utama")]
        TajukUtama = 0,
        [Display(Name = "Tajuk Kecil")]
        TajukKecil = 1,
        [Display(Name = "Perihalan")]
        Perihalan = 2
    }
}
