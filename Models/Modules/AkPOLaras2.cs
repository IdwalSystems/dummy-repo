using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPOLaras2
    {
        //field
        public int Id { get; set; }
        public int AkPOLarasId { get; set; }
        public int Indek { get; set; }
        public int Baris { get; set; }
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
        //field end

        //Relationship
        //relationship end
    }
}
