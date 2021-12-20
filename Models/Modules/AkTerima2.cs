using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkTerima2
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int JCaraBayarId { get; set; }
        public decimal Amaun { get; set; }
        [MaxLength(10)]
        public string NoCek { get; set; } 
        [MaxLength(1)]
        public string JenisCek { get; set; } 
        [MaxLength(4)]
        public string KodBankCek { get; set; } 
        [MaxLength(100)]
        public string TempatCek { get; set; }
        [MaxLength(30)]
        public string NoSlip { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarSlip { get; set; }

        

        //Relationship
        public JCaraBayar JCaraBayar { get; set; }
        //public AkTerima AkTerima { get; set; }

        // log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
    }
}