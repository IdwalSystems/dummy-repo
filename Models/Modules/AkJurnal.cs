using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkJurnal : AppLogHelper, ISoftDelete, IRecon
    {
        // note:
        // FlJenisJurnal = 0 ( Am )
        // FlJenisJurnal = 1 ( Inbois )
        // FlJenisJurnal = 2 ( Gaji )
        // FlJenisJurnal = 3 ( Pendahuluan )
        // FlJenisJurnal = 4 ( Panjar )
        // ..
        // FlKategoriPenerima = 0 ( Am / Lain - lain )
        // FlKategoriPenerima = 1 ( pembekal )
        // FlKategoriPenerima = 2 ( pekerja )
        // FlKategoriPenerima = 3 ( pemegang panjar )
        // ..
        public int Id { get; set; }
        [Required(ErrorMessage = "Jenis Kumpulan Wang Diperlukan.")]
        [DisplayName("Jenis Kumpulan Wang")]
        public int JKWId { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public int? AkTunaiRuncitId { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }

        [DisplayName("No Jurnal")]
        [MaxLength(20)]
        public string NoJurnal { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah Debit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        [Compare("JumKredit", ErrorMessage ="Pastikan jumlah debit dan jumlah kredit sama")]
        public decimal JumDebit { get; set; }
        [DisplayName("Jumlah Kredit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        [Compare("JumDebit", ErrorMessage = "Pastikan jumlah debit dan jumlah kredit sama")]
        public decimal JumKredit { get; set; }
        [MaxLength(400)]
        [Display(Name = "Catatan")]
        [Required(ErrorMessage = "Catatan Diperlukan")]
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
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        [MaxLength(15)]

        //Relationship
        [DisplayName("KW")]
        public JKW JKW { get; set; }
        public int FlJenisJurnal { get; set; }
        public int FlKategoriPenerima { get; set; }
        [DisplayName("Objek")]
        public ICollection<AkJurnal1> AkJurnal1 { get; set; }
        public ICollection<AkPadananPenyata> AkPadananPenyata { get; set; }
        public int FlTunai { get; set; }
        public DateTime? TarTunai { get; set; }
    }
}
