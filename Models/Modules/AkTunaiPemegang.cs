using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiPemegang
    {
        public int Id { get; set; }
        public int SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
    }
}
