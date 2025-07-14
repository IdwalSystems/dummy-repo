
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Operations
{
    public enum EnJenisOperasi
    {
        [Display(Name = "+")]
        Tambah = 0,
        [Display(Name = "-")]
        Tolak = 1,
        [Display(Name = "Amaun Tetap")]
        AmaunTetap = 2,
    }
}
