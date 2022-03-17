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
    public class AkTerima : AppLogHelper, ISoftDelete
    {
        // note:
        // FlJenisTerima = 0 ( Am )
        // FlJenisTerima = 1 ( Inbois )
        // FlJenisTerima = 2 ( Gaji )
        // FlJenisTerima = 3 ( Pendahuluan )
        // FlJenisTerima = 4 ( Panjar )
        // ..
        // FlKategoriPenerima = 0 ( Am / Lain - lain )
        // FlKategoriPenerima = 1 ( pembekal )
        // FlKategoriPenerima = 2 ( pekerja )
        // ..

        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan.")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [DisplayName("No Rujukan")]
        [MaxLength(20)]
        public string NoRujukan { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }
        [DisplayName("Kod Pembayar")]
        [MaxLength(20)]
        public string KodPembayar { get; set; }
        [DisplayName("No KP")]
        [MaxLength(15)]
        public string NoKp { get; set; }
        [Required(ErrorMessage = "Nama Diperlukan")]
        [MaxLength(100)]
        public string Nama { get; set; }
        [MaxLength(100)]
        public string Alamat1 { get; set; }
        [MaxLength(100)]
        public string Alamat2 { get; set; }
        [MaxLength(100)]
        public string Alamat3 { get; set; }
        [MaxLength(5)]
        public string Poskod { get; set; }
        [MaxLength(100)]
        public string Bandar { get; set; }
        [MaxLength(15)]
        public string Tel { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(400)]
        public string Sebab { get; set; }
        //field end

        //flag
        [DisplayName("Jenis Terimaan")]
        public int FlJenisTerima { get; set; }
        [DisplayName("Kategori Pembayar")]
        public int FlKategoriPembayar { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //flag end

        //Relationship
        [Required(ErrorMessage = "Jenis Kumpulan Wang Diperlukan.")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [Required(ErrorMessage = "Negeri Diperlukan.")]
        [DisplayName("Negeri")]
        public int JNegeriId { get; set; }
        public JNegeri JNegeri { get; set; }
        [Required(ErrorMessage = "Kod Bank Diperlukan")]
        [DisplayName("Kod Bank")]
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }
        [DisplayName("No Permohonan Aktiviti")]
        public int? SpPendahuluanPelbagaiId { get; set; }
        public SpPendahuluanPelbagai SpPendahuluanPelbagai { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
        //relationship end

    }
}
