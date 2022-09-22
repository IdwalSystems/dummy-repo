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
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(6)]
        public string Kod { get; set; }
        [DisplayName("No Akaun")]
        [Required(ErrorMessage = "No Akaun Diperlukan")]
        [MaxLength(20)]
        public string NoAkaun { get; set; }

        //Relationship
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kumpulan Wang")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [Required(ErrorMessage = "Bank Diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bank")]
        [DisplayName("Nama Bank")]
        public int JBankId { get; set; }
        public JBank JBank { get; set; }

        [Required(ErrorMessage = "Kod Akaun Diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kod Akaun")]
        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
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