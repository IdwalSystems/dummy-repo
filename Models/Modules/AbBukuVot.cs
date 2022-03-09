using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AbBukuVot
    {
        public int Id { get; set; }
        public string Tahun { get; set; }
        [DisplayName("KW")]
        public int JKWId { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public DateTime Tarikh { get; set; }
        public string Kod { get; set; }
        public string Penerima { get; set; }
        [DisplayName("Vot")]
        public int VotId { get; set; }
        public string Rujukan { get; set; }
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Tanggungan RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Tanggungan { get; set; }
        [DisplayName("TBS RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Tbs { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
        [DisplayName("Rizab RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Rizab { get; set; }
        [DisplayName("Liabiliti RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Liabiliti { get; set; }
        [DisplayName("Belanja RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Belanja { get; set; }

        //relationship
        public JKW JKW { get; set; }
        public AkCarta Vot { get; set; }

    }
}
