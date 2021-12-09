using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkJurnal
    {
        public int Id { get; set; }
        [DisplayName("KW")]
        public int JKWId { get; set; }
        [DisplayName("No Jurnal")]
        [MaxLength(20)]
        public string NoJurnal { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Jumlah Debit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumDebit { get; set; }
        [DisplayName("Jumlah Kredit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumKredit { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 1")]
        public string Catatan1 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 2")]
        public string Catatan2 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 3")]
        public string Catatan3 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 4")]
        public string Catatan4 { get; set; }
        [DefaultValue("0")]
        public int Posting { get; set; }
        [DefaultValue("0")]
        public int Cetak { get; set; }
        [DefaultValue("0")]
        public int Batal { get; set; }
        [MaxLength(15)]
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        public DateTime TarikhMasuk { get; set; }

        //Relationship
        public JKW JKW { get; set; }
        public ICollection<AkJurnal1> AkJurnal1 { get; set; }
    }
}
