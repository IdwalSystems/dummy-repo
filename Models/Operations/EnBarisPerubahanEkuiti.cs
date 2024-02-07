using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Operations
{
    public enum EnBarisPerubahanEkuiti
    {
        [Display(Name = "1. Baki Pada 1 Januari")]
        BakiAwal = 0,
        [Display(Name = "2. Pelarasan Tahun")]
        Pelarasan = 1,
        [Display(Name = "3. Lebihan Tahun")]
        Lebihan = 2
    }
}
