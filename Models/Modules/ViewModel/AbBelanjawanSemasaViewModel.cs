using MSNK.Models.Modules.FormModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules.ViewModel
{
    public class AbBelanjawanSemasaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Kumpulan Wang")]
        public int JKWId { get; set; }
        [Display(Name = "Bahagian")]
        public int JBahagianId { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan")]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [Display(Name = "Tarikh Hingga")]
        public DateTime TarHingga { get; set; }
        public string Objek { get; set; }
        public string Perihalan { get; set; }
        [Display(Name = "Paras")]
        public string Paras { get; set; }
        [Display(Name = "Asal RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Asal { get; set; }
        [Display(Name = "Tambah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Tambah { get; set; }
        [Display(Name = "Pindah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Pindah { get; set; }
        [Display(Name = "Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [Display(Name = "Belanja RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Belanja { get; set; }
        // TBS - Tanggungan belum bayar
        [Display(Name = "TBS RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TBS { get; set; }
        [Display(Name = "Telah Guna RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TelahGuna { get; set; }
        [Display(Name = "Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
    }
}
