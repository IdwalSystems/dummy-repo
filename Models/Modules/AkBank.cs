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
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        [Display(Name = "Jenis Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [Required(ErrorMessage = "Bank Diperlukan")]
        [Display(Name = "Nama Bank")]
        public int JBankId { get; set; }
        public JBank JBank { get; set; }
        [Required(ErrorMessage = "Kod Akaun Diperlukan")]
        public int AkCartaId { get; set; }
        [Display(Name = "Kod Akaun")]
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkCimbEFT> AkCimbEFT { get; set; }
        public ICollection<AkCimbEFT1> AkCimbEFT1 { get; set; }
        public ICollection<AkPenyataPemungut> AkPenyataPemungut { get; set; }
        public ICollection<AkBankRecon> AkBankRecon { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end

    }
}