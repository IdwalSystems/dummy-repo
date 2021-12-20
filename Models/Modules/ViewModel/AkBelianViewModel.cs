using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkBelianViewModel
    {
        public int Id { get; set; }
        public string Tahun { get; set; }
        public string NoInbois { get; set; }
        public DateTime Tarikh { get; set; }
        public decimal Jumlah { get; set; }
        public string NamaSykt { get; set; }
        public string Alamat1 { get; set; }
        public int FlCetak { get; set; }
        public int FlPosting { get; set; }
        public int FlBatal { get; set; }
    }
}
