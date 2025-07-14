using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JCaraBayar : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(2, ErrorMessage = "Input tidak boleh melebihi 2 aksara")]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Perihal Diperlukan")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Perihal { get; set; }
        //field end

        //relationship
        public ICollection<AkTerima2> akTerima2 { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<SuPekerja> SuPekerja { get; set; }
        public ICollection<SuProfil1> SuProfil1 { get; set; }
        public ICollection<AkPenyataPemungut> AkPenyataPemungut { get; set; }
        public ICollection<AkPVGanda> AkPVGanda { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //soft delete end
    }
}