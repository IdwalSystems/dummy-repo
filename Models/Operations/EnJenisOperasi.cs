using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNK.Models.Operations
{
    public enum EnJenisOperasi
    {
        [Display(Name = "+")]
        Tambah = 0,
        [Display(Name = "-")]
        Tolak = 1,
    }
}
