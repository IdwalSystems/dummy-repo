using Microsoft.AspNetCore.Mvc;
using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkNotaDebitKreditBelian : AppLogHelper, ISoftDelete, ICancel
    {
        public int Id { get; set; }
        [DisplayName("Bahagian")]
        public int JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        [DisplayName("No Rujukan")]
        [Required(ErrorMessage = "No Rujukan diperlukan")]
        public string NoRujukan { get; set; }
        
        // Tahun Belanjawan
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        [BindProperty]
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        public string Perihal { get; set; }
        [DisplayName("No Invois")]
        public int AkBelianId { get; set; }
        public AkBelian AkBelian { get; set; }
        public ICollection<AkNotaDebitKreditBelian1> AkNotaDebitKreditBelian1 { get; set; }
        public ICollection<AkNotaDebitKreditBelian2> AkNotaDebitKreditBelian2 { get; set; }
        //flag
        // nota:
        // FlJenis 0 - Nota Debit
        // FlJenis 1 - Nota Kredit
        [DisplayName("Debit/Kredit")]
        public int FlJenis { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        public DateTime? TarBatal { get; set; }
        [DisplayName("Hapus")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
    }
}
