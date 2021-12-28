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
        public string NoGaji { get; set; }
        public string Nama { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Poskod { get; set; }
        public string Bandar { get; set; }
        public int JNegeriId { get; set; }
        public string TelefonRumah { get; set; }
        public string TelefonBimbit { get; set; }
        public string Emel { get; set; }
        public int StatusKahwin { get; set; }
        public int BilAnak { get; set; }
        public decimal GajiPokok { get; set; }
        public DateTime TarikhMasukKerja { get; set; }
        public DateTime? TarikhBerhentiKerja { get; set; }
        public DateTime? TarikhPencen { get; set; }
        public int JAgamaId { get; set; }
        public int JBangsaId { get; set; }
        public int JJawatanPekerjaId { get; set; }
        public int JCaraBayarId { get; set; }

        //relationship
        public JNegeri JNegeri { get; set; }
        public JAgama JAgama { get; set; }
        public JBangsa JBangsa { get; set; }
        public JJawatanPekerja JJawatanPekerja { get; set; }
        public ICollection<SuTanggunganPekerja> SuTanggungan { get; set; }
        public JCaraBayar JCaraBayar { get; set; }
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
