using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AppLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime LgDate { get; set; }
        public string LgModule { get; set; }
        public string LgOperation { get; set; }
        public string LgNote { get; set; }
        public string NoRujukan { get; set; }
        public decimal Jumlah { get; set; }
        public string SysCode { get; set; }
    }
}
