using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Operations
{
    public enum EnKategoriJumlah
    {
        [Display(Name = "Amaun Biasa")]
        Amaun = 0,
        [Display(Name = "Jumlah Kecil")]
        JumlahKecil = 1,
        [Display(Name = "Jumlah Besar")]
        JumlahBesar = 2,
        [Display(Name = "Jumlah Keseluruhan")]
        JumlahKeseluruhan = 3
    }
}
