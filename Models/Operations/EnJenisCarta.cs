using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Operations
{
    public enum EnJenisCarta
    {
        [Display(Name = "Liabiliti")]
        LIABILITI = 5,
        [Display(Name = "Ekuiti")]
        EKUITI = 4,
        [Display(Name = "Belanja")]
        BELANJA = 3,
        [Display(Name = "Aset")]
        ASET = 2,
        [Display(Name = "Hasil")]
        HASIL = 1
    }
}