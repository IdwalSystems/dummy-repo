using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SuPekerja
    {
        public int Id { get; set; }
        [DisplayName("No Gaji")]
        public string NoGaji { get; set; }
        public string NoKp { get; set; }
        public string Nama { get; set; }
        [DisplayName("Alamat")]
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Poskod { get; set; }
        public string Bandar { get; set; }
        [DisplayName("Negeri")]
        public int JNegeriId { get; set; }
        [DisplayName("No Telefon Rumah")]
        public string TelefonRumah { get; set; }
        [DisplayName("No Telefon Bimbit")]
        public string TelefonBimbit { get; set; }
        public string Emel { get; set; }
        [DefaultValue("0")]
        [DisplayName("Status Perkahwinan")]
        public int StatusKahwin { get; set; }
        [DefaultValue("0")]
        [DisplayName("Bilangan Anak")]
        public int BilAnak { get; set; }
        [DisplayName("Gaji Pokok")]
        public decimal GajiPokok { get; set; }
        [DisplayName("Tarikh Masuk Kerja")]
        public DateTime TarikhMasukKerja { get; set; }
        [DisplayName("Tarikh Berhenti Kerja")]
        public DateTime? TarikhBerhentiKerja { get; set; }
        [DisplayName("Tarikh Pencen")]
        public DateTime? TarikhPencen { get; set; }
        [DisplayName("Nama Bank")]
        public int? JBankId { get; set; }
        [DisplayName("Agama")]
        public int? JAgamaId { get; set; }
        [DisplayName("Bangsa")]
        public int? JBangsaId { get; set; }
        [DisplayName("Jawatan")]
        public int? JJawatanPekerjaId { get; set; }
        [DisplayName("Cara Bayar")]
        public int? JCaraBayarId { get; set; }
        [DisplayName("No Akaun Bank")]
        public string NoAkaunBank { get; set; }

        //relationship
        [DisplayName("Negeri")]
        public JNegeri JNegeri { get; set; }
        [DisplayName("Agama")]
        public JAgama JAgama { get; set; }
        [DisplayName("Nama Bank")]
        public JBank JBank { get; set; }
        [DisplayName("Bangsa")]
        public JBangsa JBangsa { get; set; }
        [DisplayName("Jawatan")]
        public JJawatanPekerja JJawatanPekerja { get; set; }
        public ICollection<SuTanggunganPekerja> SuTanggungan { get; set; }
        [DisplayName("Cara Bayar")]
        public JCaraBayar JCaraBayar { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkTunaiCV> AkTunaiCV { get; set; }
        //relationship end

        // log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        //log end
    }
}
