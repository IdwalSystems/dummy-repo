using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkBankViewModel
    {
        [Required]
        public string Kod { get; set; }
        [Required]
        public string NoAkaun { get; set; }
        public int KWId { get; set; } 
        public int BankId { get; set; }
    }
}
