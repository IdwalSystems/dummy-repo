using Microsoft.AspNetCore.Mvc;
using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkInvois : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan.")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("No Rujukan")]
        [Required(ErrorMessage = "No Rujukan Diperlukan")]
        public string NoInbois { get; set; }
        [BindProperty]
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        //field end

        //flag
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }

        //flag end

        //Relationship
        [Required(ErrorMessage = "Jenis Kumpulan Wang Diperlukan.")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        [DisplayName("No Pesanan Tempatan")]
        public int? AkPOId { get; set; }
        [Required(ErrorMessage = "Kod Penghutang Diperlukan.")]
        [DisplayName("Kod Penghutang")]
        public int KodObjekAPId { get; set; }
        [Required(ErrorMessage = "Kod Penghutang Diperlukan.")]
        [DisplayName("Kod Penghutang")]
        public int AkPenghutangId { get; set; }
        public JKW JKW { get; set; }
        public AkPO AkPO { get; set; }
        public AkCarta KodObjekAP { get; set; }
        public AkPenghutang AkPenghutang { get; set; }
        public ICollection<AkInvois1> AkInvois1 { get; set; }
        public ICollection<AkInvois2> AkInvois2 { get; set; }
        public ICollection<AkTerima3> AkTerima3 { get; set; }

        //relationship end

        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
    }
}
