using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkPVViewModel
    {
        public int Id { get; set; }
        public string Tahun { get; set; }
        public string NoPV { get; set; }
        public DateTime Tarikh { get; set; }
        public decimal Jumlah { get; set; }
        public string Penerima { get; set; }
        public string CaraBayar { get; set; }
        public int FlPosting { get; set; }
        public int FlBatal { get; set; }
        public int FlCetak { get; set; }
    }
}
