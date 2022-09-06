using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkPVGanda
    {
        // note :
        // FlKategoriPenerima = 1 -- pekerja
        // FlKategoriPenerima = 2 -- atlet
        // FlKategoriPenerima = 3 -- jurulatih
        // FlKategoriPenerima = 0 -- null
        public int Id { get; set; }
        public int AkPVId { get; set; }
        public AkPV AkPV { get; set; }
        public int Indek { get; set; }
        public int FlKategoriPenerima { get; set; }
        [DisplayName("Anggota")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
        [DisplayName("Atlet")]
        public int? SuAtletId { get; set; }
        public SuAtlet SuAtlet { get; set; }
        [DisplayName("Jurulatih")]

        public int? SuJurulatihId { get; set; }

        public SuJurulatih SuJurulatih { get; set; }
        
        public string Nama { get; set; }
        [DisplayName("No KP")]
        public string NoKp {get; set; }
        [DisplayName("No Akaun")]
        public string NoAkaun { get; set; }
        [DisplayName("Bank")]
        public int JBankId { get; set; }
        public JBank JBank { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amaun { get; set; }
        [DisplayName("No Cek / EFT")]
        public string NoCekAtauEFT { get; set; }
        [DisplayName("Tar Cek / EFT")]
        public DateTime? TarCekAtauEFT { get; set; }
        [DisplayName("Cara Bayar")]
        public int JCaraBayarId { get; set; }
        public JCaraBayar JCaraBayar { get; set; }
    }
}
