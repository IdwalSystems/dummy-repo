using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkPVViewModel
    {
        public bool denganTanggungan { get; set; }
        public int Id { get; set; }
        public string Tahun { get; set; }
        public string NoPV { get; set; }
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhTerima { get; set; }
        public DateTime? TarikhPosting { get; set; }
        public JKW JKW { get; set; }
        public AkBank AkBank { get; set; }
        public decimal Jumlah { get; set; }
        public string KodPenerima { get; set; }
        public string NoKP { get; set; }
        public string Penerima { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string NoAkaunBank { get; set; }
        public string Telefon { get; set; }
        public string Emel { get; set; }
        public string CaraBayar { get; set; }
        public string NoCekAtauEFT { get; set; }
        public DateTime? TarCekAtauEFT { get; set; }
        public string Perihal { get; set; }
        public int FlPosting { get; set; }
        public int FlBatal { get; set; }
        public int FlCetak { get; set; }
        public ICollection<AkPV1> AkPV1 { get; set; }
        public ICollection<AkPV2> AkPV2 { get; set; }
        public decimal JumlahInbois { get; set; }
    }
}
