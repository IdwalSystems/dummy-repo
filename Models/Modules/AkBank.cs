using Microsoft.AspNetCore.Mvc.Rendering;
using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkBank : AppLogHelper, ISoftDelete
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

        //soft delete
        public bool FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end

    }
}