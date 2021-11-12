using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class Paras
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }
    }
}
