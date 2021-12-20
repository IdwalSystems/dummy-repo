using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkBank
    {
        public int JKWId { get; set; }
        public int JBankId { get; set; }
        public int AkCartaId { get; set; }
        public int Id { get; set; }
        [MaxLength(6)]
        public string Kod { get; set; }
        [MaxLength(20)]
        public string NoAkaun { get; set; }

        //Relationship
        public JKW JKW { get; set; }
        public JBank JBank { get; set; }
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }

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