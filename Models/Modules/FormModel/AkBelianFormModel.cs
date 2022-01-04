using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.FormModel
{
    public class AkBelianFormModel : AkBelian
    {
        public decimal JumlahObjek { get; set; }
        [BindProperty]
        public decimal JumlahPerihal { get; set; }
    }
}
