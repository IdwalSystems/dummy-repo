using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPendahuluanPelbagai2
    {
        public int Id { get; set; }
        public int SpPendahuluanPelbagaiId { get; set; }
        public int Indek { get; set; }
        public decimal Baris { get; set; }
        public string Perihal { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kadar { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bil { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bulan { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }

    }
}
