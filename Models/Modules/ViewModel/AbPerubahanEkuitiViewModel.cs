using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules.ViewModel
{
    public class AbPerubahanEkuitiViewModel
    {
        public string Perihal { get; set; }
        public string TahunSebelum { get; set; }

        [Display(Name = "Baki Tahun Sebelum")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BakiAwalTahunSebelum { get; set; }
        [Display(Name = "Pelarasan Tahun Sebelum")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PelarasanTahunSebelum { get; set; }
        [Display(Name = "Lebihan Tahun Sebelum")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal LebihanTahunSebelum { get; set; }
        //--------------------------------------------------------

        public string TahunIni { get; set; }
        [Display(Name = "Baki Awal Tahun Ini")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BakiAwalTahunIni { get; set; }
        [Display(Name = "Baki Awal Tahun Ini")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PelarasanTahunIni { get; set; }
        [Display(Name = "Lebihan Tahun Ini")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal LebihanTahunIni { get; set; }

        [Display(Name = "Baki Akhir Tahun Ini")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BakiAkhirTahunIni { get; set; }
    }
}
