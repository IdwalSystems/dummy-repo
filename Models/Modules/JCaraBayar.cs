using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JCaraBayar : AppLogHelper, ISoftDelete
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

        //relationship
        public ICollection<AkTerima2> akTerima2 { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<SuPekerja> SuPekerja { get; set; }
        //relationship end

        //soft delete
        public bool FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}