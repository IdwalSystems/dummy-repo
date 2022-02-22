using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JNegeri : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required]
        [MaxLength(2)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; }
        //field end

        //Relationship
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkPembekal> AkPembekal { get; set; }
        public ICollection<SuPekerja> SuPekerja { get; set; }
        public ICollection<SpPendahuluanPelbagai> SpPermohonanAktiviti { get; set; }
        //relationship end

        //soft delete
        public bool FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}