using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkBank
    {
        
        
        
        public int Id { get; set; }
        [MaxLength(6)]
        public string Kod { get; set; }
        [Display(Name = "No Akaun")]
        [MaxLength(20)]
        public string NoAkaun { get; set; }

        //Relationship    
        public int JKWId { get; set; }
        [Display(Name = "Jenis Kumpulan Wang")]
        public JKW JKW { get; set; }   
        public int JBankId { get; set; }
        [Display(Name = "Nama Bank")]
        public JBank JBank { get; set; }
        public int AkCartaId { get; set; }
        [Display(Name = "Kod Akaun")]
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkTunaiPanjar> AkTunaiPanjar { get; set; }

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