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
    public class SuProfil1 

    {
        public int Id { get; set; }
        public int SuProfilId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Amaun RM")]
        public decimal Amaun { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Amaun Sebelum RM")]
        public decimal AmaunSebelum { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Tunggakan RM")]
        public decimal Tunggakan { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }

        //relationship
        public SuAtlet SuAtlet { get; set; }
        [DisplayName("Nama Atlet")]
        public int? SuAtletId { get; set; }

        public SuJurulatih SuJurulatih { get; set; }
        [DisplayName("Nama Jurulatih")]
        public int? SuJurulatihId { get; set; }

        public JSukan JSukan { get; set; }
        [DisplayName("Sukan")]
        public int JSukanId { get; set; }

        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
