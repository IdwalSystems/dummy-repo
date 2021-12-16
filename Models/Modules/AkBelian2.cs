using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkBelian2
    {
        public int Id { get; set; }
        public int AkBelianId { get; set; }
        public int Indek { get; set; }
        [MaxLength(3)]
        public string Bil { get; set; }
        [MaxLength(20)]
        public string NoStok { get; set; }
        [MaxLength(100)]
        public string Perihal { get; set; }
        public decimal Kuantiti { get; set; }
        [MaxLength(100)]
        public string Unit { get; set; }
        public decimal Harga { get; set; }
        public decimal Amaun { get; set; }

        //Relationship
    }
}
