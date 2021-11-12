using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkBank
    {
        public int KWId { get; set; }
        public int BankId { get; set; }
        public int AkCartaId { get; set; }
        public int Id { get; set; }
        [MaxLength(100)]
        public string NoAkaun { get; set; }

        public KW KW { get; set; }
        public Bank Bank { get; set; }
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTerima> akTerima { get; set; }

    }
}