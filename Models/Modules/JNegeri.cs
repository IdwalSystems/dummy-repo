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
        [Required(ErrorMessage = "Kod diperlukan")]
        [MaxLength(2, ErrorMessage = "Input tidak boleh melebihi 2 aksara")]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Perihal diperlukan")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Perihal { get; set; }
        //field end

        //Relationship
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkPembekal> AkPembekal { get; set; }
        public ICollection<SuPekerja> SuPekerja { get; set; }
        public ICollection<SpPendahuluanPelbagai> SpPermohonanAktiviti { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //soft delete end
    }
}