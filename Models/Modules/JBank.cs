using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JBank : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required]
        [MaxLength(12)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nama { get; set; }
        public string KodEFT { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkPembekal> AkPembekal { get; set; }
        //field end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}