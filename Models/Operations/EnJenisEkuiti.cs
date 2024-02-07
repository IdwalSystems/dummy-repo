using System.ComponentModel.DataAnnotations;


namespace MSNK.Models.Operations
{
    public enum EnJenisLajurJadualPerubahanEkuiti
    {
        [Display(Name = "MSNK")]
        KumpWang = 0,
        [Display(Name = "Rizab")]
        Rizab = 1
    }
}
