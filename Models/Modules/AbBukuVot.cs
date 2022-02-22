using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public DateTime Tarikh { get; set; }
        public string Kod { get; set; }
        public string Penerima { get; set; }
        [DisplayName("Vot")]
        public int VotId { get; set; }
        public string Rujukan { get; set; }
        [DisplayName("Debit RM")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        public decimal Kredit { get; set; }
        [DisplayName("Tanggungan RM")]
        public decimal Tanggungan { get; set; }
        [DisplayName("TBS RM")]
        public decimal Tbs { get; set; }
        [DisplayName("Baki RM")]
        public decimal Baki { get; set; }
        [DisplayName("Rizab RM")]
        public decimal Rizab { get; set; }
        [DisplayName("Liabiliti RM")]
        public decimal Liabiliti { get; set; }
        [DisplayName("Belanja RM")]
        public decimal Belanja { get; set; }

        //relationship
        public JKW JKW { get; set; }
        public AkCarta Vot { get; set; }

    }
}
