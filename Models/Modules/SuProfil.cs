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
    public class SuProfil : AppLogHelper, ISoftDelete

    {
        public int Id { get; set; }
        public string NoRujukan { get; set; }
        public string Bulan { get; set; }
        public string Tahun { get; set; }
        //Kategori
        //  0 = Atlet 
        //  1 = Jurulatih
        //Kategori END
        [DisplayName("Kategori")]
        public int FlKategori { get; set; }
        //relationship
        [Display(Name = "Kod Akaun")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        [Display(Name = "Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        [Display(Name = "Bahagian")]
        public int JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public ICollection<SuProfil1> SuProfil1 { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
