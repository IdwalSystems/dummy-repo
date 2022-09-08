using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkPVViewModel : AkPV
    {
        public string KodPenerima { get; set; }
        public string Penerima { get; set; }
        public string CaraBayar { get; set; }
        public string BankPenerima { get; set; }
        public decimal JumlahInbois { get; set; }
        public decimal JumlahGanda { get; set; }
    }
}
