using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JKW
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; }


        //Relationship
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkAkaun> AkAkaun { get; set; }
        public ICollection<AkPO> AkPO { get; set; }
        public ICollection<AkPO1> AkPO1 { get; set; }
        public ICollection<AkJurnal> AkJurnal { get; set; }
    }
}