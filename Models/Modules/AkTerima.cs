using MSNK.Models.Helper;
using MSNK.Models.Operations;
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
        // ..
        // FlKategoriPembayar = 0 ( Am / Lain - lain )
        // FlKategoriPembayar = 1 ( penghutang )
        // FlKategoriPembayar = 2 ( pekerja )
        // ..

        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun diperlukan")]
        [MaxLength(4)]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Tahun { get; set; }
        [DisplayName("No Rujukan")]
        [MaxLength(20)]
        public string NoRujukan { get; set; }
        [Required(ErrorMessage = "Tarikh diperlukan")]
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
        [Required(ErrorMessage = "Nama diperlukan")]
        [MaxLength(100)]
        public string Nama { get; set; }
        [MaxLength(100)]
        [DisplayName("Alamat")]
        public string Alamat1 { get; set; }
        [MaxLength(100)]
        public string Alamat2 { get; set; }
        [MaxLength(100)]
        public string Alamat3 { get; set; }
        [MaxLength(5)]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Poskod { get; set; }
        [MaxLength(100)]
        public string Bandar { get; set; }
        [MaxLength(15)]
        public string Tel { get; set; }
        [EmailAddress(ErrorMessage = "Emel tidak sah"), MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(400)]
        public string Sebab { get; set; }
        //field end

        //flag
        [DisplayName("Jenis Terimaan")]
        public JenisBaucer FlJenisTerima { get; set; }
        [DisplayName("Kategori Pembayar")]
        public KategoriPembayar FlKategoriPembayar { get; set; }
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
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        [DisplayName("Kumpulan Wang")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kump. Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [Required(ErrorMessage = "Negeri diperlukan")]
        [DisplayName("Negeri")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Negeri")]
        public int JNegeriId { get; set; }
        public JNegeri JNegeri { get; set; }
        [Required(ErrorMessage = "Kod Bank diperlukan")]
        [DisplayName("Kod Bank")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kod Bank")]
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }
        [DisplayName("No Permohonan Aktiviti")]
        public int? SpPendahuluanPelbagaiId { get; set; }
        public SpPendahuluanPelbagai SpPendahuluanPelbagai { get; set; }
        [DisplayName("Penghutang")]
        public int? AkPenghutangId { get; set; }
        public AkPenghutang AkPenghutang { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
        public ICollection<AkTerima3> AkTerima3 { get; set; }
        //relationship end

    }
}
