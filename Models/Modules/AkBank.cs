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
        public int Id { get; set; }
        public KW KW { get; set; }
        [MaxLength(4)]
        public string Kod { get; set; }
        public Bank Bank { get; set; }
        
        [MaxLength(100)]
        public string NoAkaun { get; set; }


    }
}