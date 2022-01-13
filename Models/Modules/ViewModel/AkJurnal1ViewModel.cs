using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkJurnal1ViewModel
    {
        public int AkJurnalId { get; set; }
        public int IndeksLama { get; set; }
        public int IndeksBaru { get; set; }
        public int AkCartaId { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
    }
}
